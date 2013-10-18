﻿using System.Windows;
using NUnit.Framework;

namespace DeltaEngine.Editor.ParticleEditor.Tests
{
	[RequiresSTA]
	public class ParticleEditorViewTests
	{
		//ncrunch: no coverage start
		[Test, Category("Slow"), Category("WPF")]
		public void ShowParticleEditorViewInWindow()
		{
			var window = new Window
			{
				Title = "WPF Test - UserControl ParticleEditorView",
				Content = new ParticleEditorView()
			};
			window.ShowDialog();
		}
	}
}