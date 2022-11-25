﻿using FileManagerWebApi.Services.FileManagerServices;
using FileManagerWebApi.Services.LoggingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FileManagerWebApi.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly IUserInfoFileManagerService userInfoFMservice;
        private readonly ISigningUpService signingUpService;

        public FileManagerController(IUserInfoFileManagerService _userInfoFMservice, ISigningUpService _signingUpService)
        {
            userInfoFMservice = _userInfoFMservice;
            signingUpService = _signingUpService;
        }

        [Authorize("Bearer")]
        [/*HttpPost*/HttpGet("AddFile")]
        public IActionResult AddFile(string fileName, string filePath)
        {
            string gmail = (HttpContext.User.Identity as ClaimsIdentity).FindFirst("Gmail").Value;

            try
            {
                userInfoFMservice.AddFile(gmail, fileName, filePath);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("success");
        }

        [Authorize("Bearer")]
        [/*HttpPost*/HttpGet("DownloadFile")]
        public IActionResult DownloadFile(string fromGmail, string fileName, string destPath)
        {
            try
            {
                userInfoFMservice.DownloadFile(fromGmail, fileName, destPath);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("success");
        }

        [Authorize("Bearer")]
        [/*HttpPost*/HttpGet("ShareFile")]
        public IActionResult ShareFile(string[] toGmails, string[] filesName)
        {
            string gmail = (HttpContext.User.Identity as ClaimsIdentity).FindFirst("Gmail").Value;

            try
            {
                userInfoFMservice.ShareFile(gmail, toGmails, filesName);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("success");
        }

        [Authorize("Bearer")]
        [HttpGet("GetAllAcceptedFiles")]
        public IActionResult GetAllAcceptedFiles()
        {
            string gmail = (HttpContext.User.Identity as ClaimsIdentity).FindFirst("Gmail").Value;

            string[] allSharedFiles;

            try
            {
                allSharedFiles = userInfoFMservice.ShowAllAcceptedFiles(gmail);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok(allSharedFiles);
        }

        [Authorize("Bearer")]
        [/*HttpDelete*/HttpGet("DeleteFile")]
        public IActionResult DeleteFile(string fileName, bool isPersonal)
        {
            string gmail = (HttpContext.User.Identity as ClaimsIdentity).FindFirst("Gmail").Value;

            try
            {
                userInfoFMservice.DeleteFile(gmail, fileName, isPersonal);
            }
            catch(Exception e)
            {
                return Problem(e.Message);
            }

            return Ok("Success");
        }

        [Authorize("Bearer")]
        [/*HttpDelete*/HttpGet("GetAllGmails")]
        public IActionResult GetAllGmails()
        {
            return Ok(signingUpService.GetAllGmails());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
