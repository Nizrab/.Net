using Azure;
using Azure.Storage.Blobs;
using Lab5.Data;
using Lab5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Lab5.Controllers
{
    public class AnswerImagesController : Controller
    {
        private readonly AnswerImageDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";
        private string containerName;
        public AnswerImagesController(AnswerImageDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.AnswerImages.ToListAsync());
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile answerImage)
        {
            BlobContainerClient containerClient;
            if (Request.Form["Question"] == earthContainerName)
            {
                containerName = earthContainerName;
            }
            else
            {
                containerName = computerContainerName;
            }
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            try
            {
                // create the blob to hold the data
                BlobClient blockBlob = containerClient.GetBlobClient(answerImage.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }
                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await answerImage.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }
                // add the photo to the database if it uploaded successfully
                var image = new AnswerImage();
                image.Url = blockBlob.Uri.AbsoluteUri;
                image.FileName = answerImage.FileName;
                if (Request.Form["Question"] == earthContainerName)
                {
                    image.Question = Question.earth;
                }
                else
                {
                    image.Question = Question.computer;
                }
                _context.AnswerImages.Add(image);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                View("Error");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var image = await _context.AnswerImages.FirstOrDefaultAsync(m => m.AnswerImageId == id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image); 
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FullDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var image = await _context.AnswerImages.FirstOrDefaultAsync(m => m.AnswerImageId == id);
            BlobContainerClient containerClient;
            try
            {
                if(image.Question == Question.computer) { 
                    containerClient = _blobServiceClient.GetBlobContainerClient(computerContainerName);
                }
                else
                {
                    containerClient = _blobServiceClient.GetBlobContainerClient(earthContainerName);
                }
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }
            try 
            {
                BlobClient blobClient = containerClient.GetBlobClient(image.FileName);
                if(await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                }
                _context.AnswerImages.Remove(image);
                await _context.SaveChangesAsync();
                }
            catch (RequestFailedException)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}

