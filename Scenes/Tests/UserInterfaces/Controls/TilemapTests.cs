﻿using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class TilemapTests : TestWithMocksOrVisually
	{
		[Test]
		public void RenderColoredLogoTilemap()
		{
			CreateBorder();
			var tilemap = new ColoredLogoTilemap(World, Map) { DrawArea = Center };
			tilemap.Add(new FontText(FontXml.Default, "", new Rectangle(0.3f, 0.6f, 0.2f, 0.2f))
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				RenderLayer = 2
			});
			tilemap.Start<RenderDragInfo>();
		}

		//ncrunch: no coverage start
		private static readonly Size World = new Size(100, 100);
		private static readonly Size Map = new Size(10, 10);
		private static readonly Rectangle Center = new Rectangle(Left, Top, Width, Height);
		private const float Left = 0.3f;
		private const float Top = 0.3f;
		private const float Width = 0.4f;
		private const float Height = 0.4f;
		//ncrunch: no coverage end

		private static void CreateBorder()
		{
			new FilledRect(
				new Rectangle(Left - 2 * TileWidth, Top - 2 * TileHeight, BorderWidth, 2 * TileHeight),
				Color.Black) { RenderLayer = 1 };
			new FilledRect(
				new Rectangle(Left - 2 * TileWidth, Top - 2 * TileHeight, 2 * TileWidth, BorderHeight),
				Color.Black) { RenderLayer = 1 };
			new FilledRect(
				new Rectangle(Left - 2 * TileWidth, Top + (Map.Height - 1) * TileHeight, BorderWidth,
					2 * TileHeight), Color.Black) { RenderLayer = 1 };
			new FilledRect(
				new Rectangle(Left + (Map.Width - 1) * TileWidth, Top - 2 * TileHeight, 2 * TileWidth,
					BorderHeight), Color.Black) { RenderLayer = 1 };
		}

		//ncrunch: no coverage start
		private static readonly float TileWidth = Center.Width / Map.Width;
		private static readonly float TileHeight = Center.Height / Map.Height;
		private static readonly float BorderWidth = (Map.Width + 3) * TileWidth;
		private static readonly float BorderHeight = (Map.Height + 3) * TileHeight;

		private class RenderDragInfo : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (Tilemap tileMap in entities)
					tileMap.Get<FontText>().Text = "Delta: " + tileMap.State.DragDelta + "\nDragStart:" +
						tileMap.State.DragStart + "\nDragEnd:" + tileMap.State.DragEnd;
			}
		}
		//ncrunch: no coverage end
	}
}