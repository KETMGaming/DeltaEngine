﻿using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Editor.Core;
using DeltaEngine.Extensions;

namespace DeltaEngine.Editor.Mocks
{
	public class MockService : Service
	{
		public MockService(string userName, string projectName)
		{
			UserName = userName;
			ProjectName = projectName;
		}

		public string UserName { get; private set; }
		public string ProjectName { get; private set; }

		public void ReceiveData(object data)
		{
			if (DataReceived != null)
				DataReceived(data);
			if (ContentUpdated != null)
				ContentUpdated(ContentType.Scene, "MockContent");
			if (ContentDeleted != null)
				ContentDeleted("MockContent");
		}

		public event Action<object> DataReceived;
		public event Action<ContentType, string> ContentUpdated;
		public event Action<string> ContentDeleted;

		public void Send(object message, bool allowToCompressMessage = true) {}

		public IEnumerable<string> GetAllContentNames()
		{
			var list = new List<string>();
			foreach (ContentType type in EnumExtensions.GetEnumValues<ContentType>())
				list.AddRange(GetAllContentNamesByType(type));
			return list;
		}

		public IEnumerable<string> GetAllContentNamesByType(ContentType type)
		{
			var list = new List<string>();
			for (int i = 0; i < 2; i++)
			{
				string contentName = "My" + type + (i + 1);
				if (type == ContentType.Material)
					contentName += "In" + (i + 2) + "D";
				list.Add(contentName);
			}
			return list;
		}

		public ContentType? GetTypeOfContent(string content)
		{
			if ("TestUser" == content)
				return null;
			if (content.Contains("ImageAnimation"))
				return ContentType.ImageAnimation;
			if (content.Contains("SpriteSheet"))
				return ContentType.SpriteSheetAnimation;
			if (content.Contains("Mesh"))
				return ContentType.Mesh;
			return ContentType.Image;
		}

		public void UploadContent(ContentMetaData metaData,
			Dictionary<string, byte[]> optionalFileData = null)
		{
			NumberOfMessagesSend++;
		}

		public int NumberOfMessagesSend { get; private set; }

		public void DeleteContent(string selectedContent, bool deleteSubContent)
		{
			NumberOfMessagesSend++;
		}

		public void StartPlugin(Type plugin) {}
	}
}