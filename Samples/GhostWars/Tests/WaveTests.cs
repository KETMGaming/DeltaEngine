﻿using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace GhostWars.Tests
{
	public class WaveTests : TestWithMocksOrVisually
	{
		[Test]
		public void GhostWaveFromLeftToRight()
		{
			CreateWave(true);
			AdvanceTimeAndUpdateEntities();
		}

		private static void CreateWave(bool leftToRight)
		{
			var wave = new GhostWave(new Vector2D(leftToRight ? 0.2f : 0.8f, 0.5f),
				new Vector2D(leftToRight ? 0.8f : 0.2f, 0.5f), 5, Team.HumanYellow.ToColor());
			wave.TargetReached = (attacker, waveSize) => CreateWave(!leftToRight);
		}

		[Test]
		public void CreateWaveOnClick()
		{
			new Command(Command.Click,
				position =>
					new GhostWave(new Vector2D(0.2f, 0.5f), position, 5, Team.HumanYellow.ToColor()));
		}

		[Test]
		public void MultipleWaves()
		{
			new GhostWave(new Vector2D(0.2f, 0.5f), new Vector2D(0.8f, 0.5f), 5,
				Team.HumanYellow.ToColor());
			new GhostWave(new Vector2D(0.3f, 0.6f), new Vector2D(0.6f, 0.6f), 5,
				Team.ComputerTeal.ToColor());
			new GhostWave(new Vector2D(0.4f, 0.7f), new Vector2D(0.6f, 0.7f), 5,
				Team.ComputerTeal.ToColor());
		}
	}
}