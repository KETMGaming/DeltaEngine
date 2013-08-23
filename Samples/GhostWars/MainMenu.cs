﻿using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine;
using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Sprites;
using DeltaEngine.ScreenSpaces;

namespace GhostWars
{
	public class MainMenu : Entity, Updateable
	{
		public MainMenu(Window window, Mouse mouse)
		{
			this.window = window;
			this.mouse = mouse;
			CreateMainMenu();
			new Command("Exit", window.CloseAfterFrame);
		}

		private readonly Window window;
		private readonly Mouse mouse;

		private void CreateMainMenu()
		{
			Clear();
			AddMenuBackground();
			AddMenuOption(OnHowToPlay, "How to play", new Point(0.5f, 0.50f));
			AddMenuOption(OnSingleplayer, "Singleplayer", new Point(0.5f, 0.57f));
			AddMenuOption(OnCredits, "Credits", new Point(0.5f, 0.64f));
			AddMenuOption(window.CloseAfterFrame, "Exit", new Point(0.5f, 0.71f));
		}


		private void AddMenuBackground()
		{
			Add(new Sprite("MenuBackground", ScreenSpace.Current.Viewport)
			{
				RenderLayer = int.MinValue
			});
			Add(new Sprite("Logo", Rectangle.FromCenter(new Point(0.5f, 0.3f), new Size(0.2f))));
		}

		private void Clear()
		{
			if (entities.Count > 0)
				PlayClickSound();
			foreach (var entity in entities)
				entity.IsActive = false;
			entities.Clear();
		}

		private static void PlayClickSound()
		{
			ContentLoader.Load<Sound>("MalletHit").Play();
		}

		private readonly List<Entity> entities = new List<Entity>();

		private void Add(Entity entity)
		{
			entities.Add(entity);
		}

		private void AddMenuOption(Action clickAction, string buttonText, Point position)
		{
			var buttonRect = Rectangle.FromCenter(position, new Size(0.29f, 0.0525f));
			Add(new Sprite("ButtonBackground", buttonRect));
			Add(new FontText(Font, buttonText, buttonRect));
			Add(new Command(Command.Click, point =>
			{
				if (buttonRect.Contains(point))
					clickAction();
			}));
		}

		private void OnHowToPlay()
		{
			Clear();
			Add(new Sprite("GhostWarsHowToPlay", ScreenSpace.Current.Viewport));
			Add(new Command(Command.Click, CreateMainMenu));
		}

		private void OnSingleplayer()
		{
			Clear();
			AddMenuBackground();
			Add(new FontText(Font, "Level Selection", new Point(0.5f, 0.5f)));
			AddLevelSelection(1, Rectangle.FromCenter(0.25f, 0.66f, 0.19f, 0.19f));
			AddLevelSelection(2, Rectangle.FromCenter(0.5f, 0.66f, 0.19f, 0.19f));
			AddLevelSelection(3, Rectangle.FromCenter(0.75f, 0.66f, 0.19f, 0.19f));
		}

		private void AddLevelSelection(int levelNumber, Rectangle mapDrawArea)
		{
			var levelText = new FontText(Font, "Level " + levelNumber,
				Rectangle.FromCenter(mapDrawArea.Center - new Point(0.0f, 0.115f), new Size(0.2f, 0.1f)));
			Add(levelText);
			var map = new Sprite("GhostWarsLevel" + levelNumber, mapDrawArea);
			Add(map);
			Add(new Command(Command.Click, position =>
			{
				if (levelText.DrawArea.Contains(position) || map.DrawArea.Contains(position))
					StartGame(levelNumber);
			}));
		}

		private void StartGame(int level)
		{
			Clear();
			State = GameState.CountDown;
			Add(new Sprite("Background", ScreenSpace.Current.Viewport) { RenderLayer = -100 });
			trees = new TreeManager(Team.HumanYellow);
			if (level == 1)
				SetupLevel1Trees();
			else if (level == 2)
				SetupLevel2Trees();
			else
				SetupLevel3Trees();
		}

		private TreeManager trees;

		private void SetupLevel1Trees()
		{
			trees.AddTree(new Point(0.11f, 0.5f), Team.HumanYellow);
			trees.AddTree(new Point(0.2f, 0.4f), Team.None);
			trees.AddTree(new Point(0.12f, 0.675f), Team.None);
			trees.AddTree(new Point(0.365f, 0.45f), Team.None);
			trees.AddTree(new Point(0.265f, 0.6f), Team.None);
			trees.AddTree(new Point(0.9f, 0.675f), Team.ComputerPurple);
			trees.AddTree(new Point(0.89f, 0.5f), Team.None);
			trees.AddTree(new Point(0.91f, 0.325f), Team.ComputerTeal);
			trees.AddTree(new Point(0.74f, 0.325f), Team.None);
			trees.AddTree(new Point(0.73f, 0.675f), Team.None);
		}

		private void SetupLevel2Trees()
		{
			trees.AddTree(new Point(0.325f, 0.55f), Team.HumanYellow);
			trees.AddTree(new Point(0.225f, 0.4f), Team.None);
			trees.AddTree(new Point(0.225f, 0.7f), Team.None);
			trees.AddTree(new Point(0.575f, 0.41f), Team.None);
			trees.AddTree(new Point(0.575f, 0.68f), Team.None);
			trees.AddTree(new Point(0.725f, 0.42f), Team.ComputerPurple);
			trees.AddTree(new Point(0.85f, 0.56f), Team.None);
			trees.AddTree(new Point(0.685f, 0.53f), Team.None);
			trees.AddTree(new Point(0.725f, 0.67f), Team.ComputerTeal);
		}

		private void SetupLevel3Trees()
		{
			trees.AddTree(new Point(0.14f, 0.45f), Team.HumanYellow);
			trees.AddTree(new Point(0.5f, 0.4f), Team.None);
			trees.AddTree(new Point(0.8f, 0.45f), Team.ComputerPurple);
			trees.AddTree(new Point(0.85f, 0.62f), Team.None);
			trees.AddTree(new Point(0.715f, 0.549f), Team.None);
			trees.AddTree(new Point(0.46f, 0.67f), Team.ComputerTeal);
			trees.AddTree(new Point(0.25f, 0.56f), Team.None);
		}

		private void OnCredits()
		{
			Clear();
			AddMenuBackground();
			Add(new FontText(Font, "Credits", new Point(0.5f, 0.45f)));
			Add(new FontText(Font, "Art", new Point(0.4f, 0.54f)) { Color = Color.Orange });
			Add(new FontText(Font, "Kirsten Grobe-Nitschke", new Point(0.7f, 0.54f))
			{
				Color = Color.Orange
			});
			Add(new FontText(Font, "Programming", new Point(0.3f, 0.60f)) { Color = Color.Orange });
			Add(new FontText(Font, "Benjamin Nitschke", new Point(0.65f, 0.60f))
			{
				Color = Color.Orange
			});
			Add(new FontText(Font, "For Delta Engine Game Jam May 2013",
				new Point(0.5f, 0.75f)) { Color = Color.Grey });
			Add(new Command(Command.Click, CreateMainMenu));
		}

		public void Update()
		{
			foreach (var fontText in entities.OfType<FontText>())
				ShowHoverEffectAndPlaySoundForFontText(fontText);
		}

		private void ShowHoverEffectAndPlaySoundForFontText(FontText fontText)
		{
			var oldColor = fontText.Color;
			fontText.Color = fontText.DrawArea.Contains(mouse.Position) ? Color.Yellow : Color.White;
			if (oldColor == fontText.Color || Time.Total - lastSwingSound < 0.075f)
				return;
			lastSwingSound = Time.Total;
			ContentLoader.Load<Sound>("MalletSwing").Play(0.24f);
		}

		private float lastSwingSound;

		public static GameState State { get; set; }
		public static Team PlayerTeam { get; set; }
		public static FontXml Font
		{
			get { return ContentLoader.Load<FontXml>("Tahoma30"); }
		}
	}
}