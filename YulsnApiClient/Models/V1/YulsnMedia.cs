﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace YulsnApiClient.Models.V1
{
    public class ReadYulsnMedia
    {
        /// <summary>The ID of the media</summary>
        public int Id { get; set; }
        /// <summary>The name of the media</summary>
        public string Name { get; set; }
        /// <summary>The content type of the media</summary>
        public string ContentType { get; set; }
        /// <summary>The media's size in bytes</summary>
        public long SizeInBytes { get; set; }
        /// <summary>The media's public URL</summary>
        public string PublicUrl { get; set; }
        /// <summary>The media's Thumbnail URL</summary>
        public string ThumbnailUrl { get; set; }
        /// <summary>The Datetimeoffset when the Media was last modified</summary>
        public DateTimeOffset LastModified { get; set; }
    }

    public class CreateYulsnMedia
    {
        /// <summary>(Friendly) Name of the media</summary>
        public string Name { get; set; }
        /// <summary>The Id of the folder where the media is going to be stored.</summary>
        public int FolderId { get; set; }
        /// <summary>The media's content type e.g. image/jpeg</summary>
        public string ContentType { get; set; }
        /// <summary>The media file as byte array</summary>
        public byte[] Media { get; set; }
        /// <summary>Store the file as uploaded, without the platform's image optimization</summary>
        public bool SkipOptimize { get; set; }
    }

    /// <summary>The kinds of folders. Sent as its name, the way the v1 API expects it.</summary>
    public enum YulsnFolderType
    {
        EmailCampaign,
        Segment,
        EmailTemplate,
        Converter,
        Media,
        ShortUrl,
        Banner
    }

    public class ReadYulsnFolder
    {
        /// <summary>Folder Id</summary>
        public int Id { get; set; }
        /// <summary>Folder name</summary>
        public string Name { get; set; }
        /// <summary>Folder type</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public YulsnFolderType Type { get; set; }
        /// <summary>If the folder is a subfolder - the Id of the parent folder</summary>
        public int? FolderId { get; set; }
        /// <summary>When the folder was last modified</summary>
        public DateTimeOffset LastModified { get; set; }
    }

    public class CreateYulsnFolder
    {
        /// <summary>Folder name, 1-255 characters</summary>
        public string Name { get; set; }
        /// <summary>Folder type</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public YulsnFolderType Type { get; set; }
        /// <summary>If the folder is a subfolder - the Id of the parent folder. The parent folder must be of the same type</summary>
        public int? FolderId { get; set; }
    }
}
