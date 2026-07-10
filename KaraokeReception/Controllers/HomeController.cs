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
        var viewModel = new RoomSearchViewModel
        {
            Input = new RoomSearchInputModel
            {
                AdultCount = 2,
                StudentCount = 0,
                SeniorCount = 0,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddHours(2)
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
    /// エラー画面を表示する。
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
