﻿using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class ComplexMenuTutorial : TestWithMocksOrVisually
	{
		[Test]
		public void DisplayComplexMenu()
		{
			CreateMainMenu();
			CreateOptionsMenu();
			CreateEulaMenu();
			CreateErrorWindow();
		}

		private void CreateMainMenu()
		{
			mainMenu.Add(new FilledRect(new Rectangle(0.2f, 0.3f, 0.4f, 0.35f), Color.Grey));
			AddShowOptionsMenuButton();
			AddShowEulaButton();
		}

		private readonly Scene mainMenu = new Scene();

		private void AddShowOptionsMenuButton()
		{
			var launchOptionsMenu = new InteractiveButton(new Rectangle(0.3f, 0.35f, 0.2f, 0.1f),
				"Set Options") { RenderLayer = 1 };
			launchOptionsMenu.Clicked += () =>
			{
				optionsMenu.Show();
				mainMenu.ToBackground();
			};
			mainMenu.Add(launchOptionsMenu);
		}

		private readonly Scene optionsMenu = new Scene();

		private void AddShowEulaButton()
		{
			var launchEulaMenu = new InteractiveButton(new Rectangle(0.3f, 0.5f, 0.2f, 0.1f),
				"Read EULA") { RenderLayer = 1 };
			launchEulaMenu.Clicked += () =>
			{
				eulaMenu.Show();
				mainMenu.ToBackground();
			};
			mainMenu.Add(launchEulaMenu);
		}

		private readonly Scene eulaMenu = new Scene();

		private void CreateOptionsMenu()
		{
			AddBackgroundToOptionsMenu();
			AddResolutionDropdownListToOptionsMenu();
			AddVolumeControlToOptionsMenu();
			AddExitButtonToOptionsMenu();
			optionsMenu.Hide();
		}

		private void AddBackgroundToOptionsMenu()
		{
			optionsMenu.Add(new FilledRect(new Rectangle(0.4f, 0.275f, 0.5f, 0.4f), Color.DarkGray)
			{
				RenderLayer = 3
			});
		}

		private void AddResolutionDropdownListToOptionsMenu()
		{
			optionsMenu.Add(new DropdownList(new Rectangle(0.45f, 0.325f, 0.4f, 0.05f),
				CreateResolutionOptions()) { MaxDisplayCount = 3, RenderLayer = 4 });
		}

		private static List<object> CreateResolutionOptions()
		{
			return new List<object>
			{
				"Full Screen",
				"800 x 600",
				"1024 x 768",
				"1280 x 1024",
				"1600 x 1200",
				"1920 x 1080"
			};
		}

		private void AddVolumeControlToOptionsMenu()
		{
			var volume = new FontText(FontXml.Default, "Volume: 100",
				new Rectangle(0.45f, 0.475f, 0.2f, 0.05f))
			{
				HorizontalAlignment = HorizontalAlignment.Left,
				RenderLayer = 4
			};
			var volumeSlider = new Slider(new Rectangle(0.65f, 0.475f, 0.2f, 0.05f)) { RenderLayer = 4 };
			volumeSlider.ValueChanged += value => volume.Text = "Volume: " + value;
			optionsMenu.Add(volume);
			optionsMenu.Add(volumeSlider);
		}

		private void AddExitButtonToOptionsMenu()
		{
			var exit = new InteractiveButton(new Rectangle(0.6f, 0.575f, 0.1f, 0.05f), "Exit")
			{
				RenderLayer = 4
			};
			exit.Clicked += () =>
			{
				optionsMenu.Hide();
				mainMenu.ToForeground();
			};
			optionsMenu.Add(exit);
		}

		private void CreateEulaMenu()
		{
			AddBackgroundToEulaMenu();
			var eula = AddEulaBoxToEulaMenu();
			AddExitButtonToEulaMenu(eula);
			eulaMenu.Hide();
		}

		private void AddBackgroundToEulaMenu()
		{
			eulaMenu.Add(new FilledRect(new Rectangle(0.4f, 0.275f, 0.4f, 0.4f), Color.DarkGray)
			{
				RenderLayer = 3
			});
		}

		private Eula AddEulaBoxToEulaMenu()
		{
			var eula = new Eula(new Rectangle(0.45f, 0.325f, 0.3f, 0.05f), CreateEula())
			{
				MaxDisplayCount = 4,
				RenderLayer = 4
			};
			eulaMenu.Add(eula);
			return eula;
		}

		private void AddExitButtonToEulaMenu(Eula eula)
		{
			var exit = new InteractiveButton(new Rectangle(0.5f, 0.575f, 0.2f, 0.05f), "Exit")
			{
				RenderLayer = 4
			};
			exit.Clicked += () =>
			{
				if (eula.WasRead)
				{
					eulaMenu.Hide();
					mainMenu.ToForeground();
				}
				else
				{
					eulaMenu.ToBackground();
					errorWindow.Show();
				}
			};
			eulaMenu.Add(exit);
		}

		private readonly Scene errorWindow = new Scene();

		private void CreateErrorWindow()
		{
			var exit = new InteractiveButton(new Rectangle(0.55f, 0.3f, 0.4f, 0.3f),
				"Must read the EULA to the end!") { RenderLayer = 9 };
			exit.Clicked += () =>
			{
				errorWindow.Hide();
				eulaMenu.ToForeground();
			};
			errorWindow.Add(exit);
			errorWindow.Hide();
		}

		private static List<object> CreateEula()
		{
			var lines = new List<object>();
			for (int i = 1; i <= 50; i++)
				lines.Add(i + ". Blah blah blah");
			return lines;
		}

		private class Eula : SelectBox
		{
			public Eula(Rectangle firstLineDrawArea, List<object> values)
				: base(firstLineDrawArea, values) {}

			public override void Update()
			{
				base.Update();
				if (scrollbar.RightValue == scrollbar.MaxValue)
					WasRead = true;
			}

			public bool WasRead { get; private set; }
		}
	}
}