﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

using EverythingNBA.Services.Utilities;
using System.Linq;

namespace EverythingNBA.Services.Implementations
{
    public class CloduinaryService : ICloudinaryService
    {
        private Cloudinary cloudinary;

        public CloduinaryService()
        {
            this.StartCloudinary();
        }

        public async Task DeleteImages(params string[] publicIds)
        {
            var deleteParams = new DelResParams
            {
                PublicIds = publicIds.ToList(),
                Invalidate = true
            };

            await cloudinary.DeleteResourcesAsync(deleteParams);
        }

        public string GetImageThumbnailURL(string thumbnailPublicId)
        {
            var imageURL = cloudinary.Api.UrlImgUp.Transform(new Transformation().Height(200).Width(200).Crop("thumb"))
                .BuildUrl(string.Format(thumbnailPublicId));

            return imageURL;
        }

        public string GetImageURL(string publicId)
        {
            var imageURL = this.cloudinary.Api.UrlImgUp.BuildUrl(publicId);

            return imageURL;
        }

        public async Task<ImageUploadResult> UploadFormFileAsync(IFormFile file)
        {
            using var memoryStream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(Guid.NewGuid().ToString(), memoryStream),
                PublicId = $"nba{Guid.NewGuid()}",
                Transformation = new Transformation().Crop("limit").Width(800).Height(600),
                EagerTransforms = new List<Transformation>()
                {
                    new Transformation().Width(200).Height(200).Crop("thumb")
                }
            };

            var result = await cloudinary.UploadAsync(uploadParams);

            return result;
        }

        private void StartCloudinary()
        {
            Account account = new Account()
            {
                Cloud = CloudConfiguration.CloudName,
                ApiKey = CloudConfiguration.APIKey,
                ApiSecret = CloudConfiguration.APIPass
            };

            Cloudinary cloudinary = new Cloudinary(account);
        }
    }
}