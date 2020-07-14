using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JobBoardWebApp.Models;
using JobBoardWebApp.Helper;
using JobBoardModels.Entities;
using System.Net.Http;
using Newtonsoft.Json;

namespace JobBoardWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        JobAPI _api = new JobAPI();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Job> jobList = new List<Job>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/job");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;

                jobList = JsonConvert.DeserializeObject<List<Job>>(result);
            }

            return View(jobList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job jobForm)
        {
            HttpClient client = _api.Initial();
            var postTask = client.PostAsJsonAsync<Job>("api/job", jobForm);

            postTask.Wait();

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(int JobId)
        {
            Job jobCon = new Job();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/job/" + JobId);

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;

                jobCon = JsonConvert.DeserializeObject<Job>(result);
            }

            return View(jobCon);
        }

        [HttpPost]
        public IActionResult Edit(Job jobForm)
        {
            HttpClient client = _api.Initial();
            var putTask = client.PutAsJsonAsync<Job>("api/job/" + jobForm.JobId, jobForm);

            putTask.Wait();

            var result = putTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int JobId)
        {
            HttpClient client = _api.Initial();
            var deleteTask = client.DeleteAsync("api/job/" + JobId);

            deleteTask.Wait();

            var result = deleteTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
