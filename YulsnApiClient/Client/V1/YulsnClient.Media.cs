﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YulsnApiClient.Models.V1;

namespace YulsnApiClient.Client
{
    partial class YulsnClient
    {
        /// <summary>Uploads a media file to a media folder</summary>
        public Task<ReadYulsnMedia> CreateMediaAsync(CreateYulsnMedia createModel) =>
            SendAsync<ReadYulsnMedia>(HttpMethod.Post, "api/v1/Media", createModel);

        /// <summary>Returns the media in a media folder</summary>
        public Task<List<ReadYulsnMedia>> GetMediaInFolderAsync(int folderId) =>
            SendAsync<List<ReadYulsnMedia>>($"api/v1/Media?folderId={folderId}");

        /// <summary>Returns a single media</summary>
        public Task<ReadYulsnMedia> GetMediaAsync(int mediaId) =>
            SendAsync<ReadYulsnMedia>($"api/v1/Media/{mediaId}");

        /// <summary>Renames a media</summary>
        public Task<ReadYulsnMedia> RenameMediaAsync(int mediaId, string newName) =>
            SendAsync<ReadYulsnMedia>(HttpMethod.Post, $"api/v1/Media/{mediaId}/Rename?newName={Uri.EscapeDataString(newName)}");

        /// <summary>Deletes a media</summary>
        public Task DeleteMediaAsync(int mediaId) =>
            SendAsync<object>(HttpMethod.Delete, $"api/v1/Media/{mediaId}");

        /// <summary>Returns all media folders as a flat list; FolderId is the parent folder</summary>
        public Task<List<ReadYulsnFolder>> GetMediaFoldersAsync() =>
            SendAsync<List<ReadYulsnFolder>>("api/v1/Media/Folders");

        /// <summary>Returns a single folder (of any type)</summary>
        public Task<ReadYulsnFolder> GetFolderAsync(int folderId) =>
            SendAsync<ReadYulsnFolder>($"api/v1/Folders/{folderId}");

        /// <summary>Creates a folder. Use <see cref="YulsnFolderType.Media"/> for a media folder</summary>
        public Task<ReadYulsnFolder> CreateFolderAsync(CreateYulsnFolder createModel) =>
            SendAsync<ReadYulsnFolder>(HttpMethod.Post, "api/v1/Folders", createModel);
    }
}
