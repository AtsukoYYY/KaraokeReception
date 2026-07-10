using System.Diagnostics;
using KaraokeReception.Domain.Entities;
using KaraokeReception.Domain.Services.PriceCalculator;
using KaraokeReception.Domain.ValueObjects;
using KaraokeReception.Infrastructure.Repositories;
using KaraokeReception.Models;
using KaraokeReception.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace KaraokeReception.Controllers;

/// <summary>
/// 利用者向けの予約受付画面を扱うコントローラー。
/// </summary>
public class HomeController : Controller
{
    private readonly PriceCalculator _priceCalculator;
    private readonly IRoomRepository _roomRepository;

    public HomeController(
        PriceCalculator priceCalculator,
        IRoomRepository roomRepository)
    {
        _priceCalculator = priceCalculator;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// 予約条件入力画面を表示する。
    /// </summary>
    public IActionResult Index()
    {
        var now = DateTime.Now;
        var viewModel = new RoomSearchViewModel
        {
            Input = new RoomSearchInputModel
            {
                AdultCount = 2,
                StudentCount = 0,
                SeniorCount = 0,
                StartTime = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    0),
                EndTime = new DateTime(
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour + 2,
                    now.Minute,
                    0),
            }
        };

        return View(viewModel);
    }

    /// <summary>
    /// 入力された予約条件を検証し、空室検索結果画面へ遷移する。
    /// </summary>
    /// <param name="input">空室検索フォームの入力値。</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(RoomSearchInputModel input)
    {
        var viewModel = new RoomSearchViewModel
        {
            Input = input
        };

        if (input.TotalPersonCount <= 0)
        {
            ModelState.AddModelError(
                string.Empty,
                "利用人数は1人以上で入力してください。");
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            // 値オブジェクトに入るかどうかの入力値チェックだけ実施
            _ = new PersonCount(input.TotalPersonCount);
            _ = new UsageTime(input.StartTime, input.EndTime);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(viewModel);
        }

        return RedirectToAction(
            nameof(SearchResults),
            new
            {
                input.AdultCount,
                input.StudentCount,
                input.SeniorCount,
                input.StartTime,
                input.EndTime
            });
    }

    /// <summary>
    /// 予約条件に合う空室候補を表示する。
    /// </summary>
    /// <param name="input">空室検索条件。</param>
    public async Task<IActionResult> SearchResults(RoomSearchInputModel input)
    {
        try
        {
            var personCount = new PersonCount(input.TotalPersonCount);
            var usageTime = new UsageTime(input.StartTime, input.EndTime);
            var rooms = await _roomRepository.GetAllAsync();

            var viewModel = new RoomSearchResultViewModel
            {
                SearchCondition = input,
                TotalUsageTime = FormatUsageTime(usageTime),
                AvailableRooms = rooms
                    .Where(room => room.Capacity.Value >= personCount.Value)
                    .Select(room => CreateAvailableRoomViewModel(
                        room,
                        usageTime,
                        personCount,
                        input.StudentCount,
                        input.SeniorCount))
                    .ToList()
            };

            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            var viewModel = new RoomSearchViewModel
            {
                Input = input
            };

            ModelState.AddModelError(string.Empty, ex.Message);
            return View(nameof(Index), viewModel);
        }
    }

    /// <summary>
    /// 選択された部屋と利用条件から予約確認画面を表示する。
    /// </summary>
    /// <param name="input">予約確認に必要な入力値。</param>
    public async Task<IActionResult> Confirm(ReservationConfirmInputModel input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input.ReservationId))
            {
                input.ReservationId = ReservationId.New().Value;
            }

            var viewModel = await CreateReservationConfirmViewModelAsync(input);

            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            var viewModel = new RoomSearchViewModel
            {
                Input = input
            };

            ModelState.AddModelError(string.Empty, ex.Message);
            return View(nameof(Index), viewModel);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// 予約を確定し、予約完了画面を表示する。
    /// </summary>
    /// <param name="input">予約確定に必要な入力値。</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(ReservationConfirmInputModel input)
    {
        try
        {
            var roomUsagePlan = await CreateRoomUsagePlanAsync(input);
            var reservationId = new ReservationId(input.ReservationId);
            var reservation = new Reservation(
                reservationId,
                roomUsagePlan);
            var price = _priceCalculator.Calculate(reservation.UsagePlan);

            var viewModel = new ReservationCompleteViewModel
            {
                ReservationId = reservation.Id.Value,
                ReservationDisplayId = reservation.Id.ToDisplayString(),
                RoomId = reservation.UsagePlan.Room.Id.Value,
                RoomName = reservation.UsagePlan.Room.Name,
                TotalUsageTime = FormatUsageTime(reservation.UsagePlan.UsageTime),
                EstimatedPriceNoTax = price.ToStringNoTax(),
                EstimatedPriceIncludeTax = price.ToStringIncludeTax()
            };

            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(nameof(Confirm), await CreateReservationConfirmViewModelAsync(input));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }



    /// <summary>
    /// 部屋情報と予約条件から、空室検索結果に表示する部屋情報を作成する。
    /// </summary>
    /// <param name="room">検索結果に表示する部屋。</param>
    /// <param name="usageTime">利用時間。</param>
    /// <param name="personCount">利用人数。</param>
    /// <param name="studentCount">学生人数。</param>
    /// <param name="seniorCount">シニア人数。</param>
    /// <returns>空室検索結果用の部屋表示モデル。</returns>
    private AvailableRoomViewModel CreateAvailableRoomViewModel(
        Room room,
        UsageTime usageTime,
        PersonCount personCount,
        int studentCount,
        int seniorCount)
    {
        var roomUsagePlan = new RoomUsagePlan
        {
            Room = room,
            UsageTime = usageTime,
            PersonCount = personCount,
            StudentCount = studentCount,
            SeniorCount = seniorCount
        };

        var price = _priceCalculator.Calculate(roomUsagePlan);

        return new AvailableRoomViewModel
        {
            RoomId = room.Id.Value,
            RoomName = room.Name,
            Capacity = $"{room.Capacity.Value}人",
            MachineType = room.MachineType.ToString(),
            EstimatedPriceNoTax = price.ToStringNoTax(),
            EstimatedPriceIncludeTax = price.ToStringIncludeTax()
        };
    }

    /// <summary>
    /// 予約確認画面に表示する予約内容を作成する。
    /// </summary>
    /// <param name="input">予約確認に必要な入力値。</param>
    /// <returns>予約確認画面の表示モデル。</returns>
    private async Task<ReservationConfirmViewModel> CreateReservationConfirmViewModelAsync(
        ReservationConfirmInputModel input)
    {
        var roomUsagePlan = await CreateRoomUsagePlanAsync(input);
        var availableRoom = CreateAvailableRoomViewModel(
            roomUsagePlan.Room,
            roomUsagePlan.UsageTime,
            roomUsagePlan.PersonCount,
            roomUsagePlan.StudentCount,
            roomUsagePlan.SeniorCount);

        return new ReservationConfirmViewModel
        {
            Input = input,
            RoomId = availableRoom.RoomId,
            RoomName = availableRoom.RoomName,
            Capacity = availableRoom.Capacity,
            MachineType = availableRoom.MachineType,
            StartTime = roomUsagePlan.UsageTime.Start.ToString("yyyy/MM/dd HH:mm"),
            EndTime = roomUsagePlan.UsageTime.End.ToString("yyyy/MM/dd HH:mm"),
            TotalUsageTime = FormatUsageTime(roomUsagePlan.UsageTime),
            EstimatedPriceNoTax = availableRoom.EstimatedPriceNoTax,
            EstimatedPriceIncludeTax = availableRoom.EstimatedPriceIncludeTax
        };
    }

    /// <summary>
    /// 利用時間を画面表示用の文字列に変換する。
    /// </summary>
    /// <param name="usageTime">利用時間。</param>
    /// <returns>利用時間の表示文字列。</returns>
    private static string FormatUsageTime(UsageTime usageTime)
    {
        var hours = usageTime.TotalMinutes / 60;
        var minutes = usageTime.TotalMinutes % 60;

        return $"{hours}時間{minutes}分";
    }

    /// <summary>
    /// 入力値から部屋利用計画を作成する。
    /// </summary>
    /// <param name="input">部屋利用計画を作成するための入力値。</param>
    /// <returns>部屋利用計画。</returns>
    /// <exception cref="InvalidOperationException">指定された部屋が存在しない場合に発生する。</exception>
    private async Task<RoomUsagePlan> CreateRoomUsagePlanAsync(
        ReservationConfirmInputModel input)
    {
        var roomId = new RoomId(input.RoomId);
        var personCount = new PersonCount(input.TotalPersonCount);
        var usageTime = new UsageTime(input.StartTime, input.EndTime);
        var rooms = await _roomRepository.GetAllAsync();
        var room = rooms.SingleOrDefault(room => room.Id == roomId);

        if (room is null)
        {
            throw new InvalidOperationException("指定された部屋が見つかりません。");
        }

        return new RoomUsagePlan
        {
            Room = room,
            UsageTime = usageTime,
            PersonCount = personCount,
            StudentCount = input.StudentCount,
            SeniorCount = input.SeniorCount
        };
    }

    /// <summary>
    /// エラー画面を表示する。
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
