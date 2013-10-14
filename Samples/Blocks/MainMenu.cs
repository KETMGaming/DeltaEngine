﻿using System;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Scenes;
using DeltaEngine.Scenes.UserInterfaces.Controls;

namespace Blocks
{
	class MainMenu : Scene
	{
		public MainMenu()
		{
			CreateMenuTheme();
			AddStartButton();
			AddQuitButton();
		}

		private void CreateMenuTheme()
		{
			SetQuadraticBackground("BlocksMainMenuBackground");
			menuTheme = new Theme();
			menuTheme.Button = new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonDefault"));
			menuTheme.ButtonMouseover = new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonHover"));
			menuTheme.ButtonPressed = new Theme.Appearance(new Material(Shader.Position2DUV, "BlocksButtonPressed"));
			menuTheme.ButtonDisabled = new Theme.Appearance();
		}

		private Theme menuTheme;

		private void AddStartButton()
		{
			var startButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.4f, 0.4f, 0.15f), "Start Game");
			startButton.Clicked += TryInvokeGameStart;
			Add(startButton);
		}

		private void TryInvokeGameStart()
		{
			if (InitGame != null)
				InitGame();
		}

		public event Action InitGame;

		private void AddQuitButton()
		{
			var quitButton = new InteractiveButton(menuTheme, new Rectangle(0.3f, 0.6f, 0.4f, 0.15f), "Quit Game");
			quitButton.Clicked += TryInvokeQuit;
			Add(quitButton);
		}

		private void TryInvokeQuit()
		{
			if (QuitGame != null)
				QuitGame();
		}

		public event Action QuitGame;
	}
}