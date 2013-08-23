﻿using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Platforms.Mocks;
using DeltaEngine.Rendering.Sprites.Tests;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Particles.Tests
{
	internal class ParticleEmitterTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateEmitterAndKeepRunning()
		{
			CreateDataAndEmitter(512, 0.01f, 5);
			emitter.Center = new Point(0.5f, 0.7f);
			new PerformanceTests.FpsDisplay();
		}

		private void CreateDataAndEmitter(int maxParticles = 1, float spawnInterval = 0.01f,
			float lifeTime = 0.2f)
		{
			effectData = new ParticleEffectData
			{
				MaximumNumberOfParticles = maxParticles,
				SpawnInterval = spawnInterval,
				LifeTime = lifeTime,
				Size = new RangeGraph<Size>(new Size(0.02f), new Size(0.06f)),
				Color = new RangeGraph<Color>(Color.Red, new Color(Color.Orange, 128)),
				Force = new RangeGraph<Point>(new Point(0, 0.1f), new Point(0.2f, 0.1f)),
				StartVelocity = new RangeGraph<Point>(new Point(0, -0.3f), new Point(0.05f, 0.01f)),
				StartRotation = new ValueRange(20f,100f),
				RotationForce = new ValueRange(0,50),
				ParticleMaterial = new Material(Shader.Position2DColorUv, "DeltaEngineLogo"),
				StartPosition = new RangeGraph<Point>(new Point(-0.1f, -0.1f), new Point(0.1f, 0.1f))
			};
			emitter = new ParticleEmitter(effectData, Point.Half);
		}

		private ParticleEffectData effectData;
		private ParticleEmitter emitter;

		[Test]
		public void CreateEmitterWithJustOneParticle()
		{
			CreateDataAndEmitter(1, 0.01f, 5);
			emitter.Center = new Point(0.5f, 0.7f);
			RunAfterFirstFrame(() =>
			{
				//Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame);
				//Assert.AreEqual(4, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame);
			});
		}

		[Test, CloseAfterFirstFrame]
		public void AdvanceCreatingOneParticle()
		{
			CreateDataAndEmitter();
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(emitter.NumberOfActiveParticles, 1);
		}

		[Test]
		public void MultipleEmittersShallNotInterfere()
		{
			CreateDataAndEmitter(12, 0.1f, 5);
			var data = new ParticleEffectData
			{
				MaximumNumberOfParticles = 12,
				SpawnInterval = 0.4f,
				LifeTime = 2,
				Size = new RangeGraph<Size>(new Size(0.03f), new Size(0.03f)),
				Color = new RangeGraph<Color>(Color.Grey, Color.Grey),
				Force = new RangeGraph<Point>(-Point.UnitY, -Point.UnitY),
				StartVelocity = new RangeGraph<Point>(new Point(0.3f, -0.1f), new Point(0.3f, -0.1f)),
				ParticleMaterial = new Material(Shader.Position2DColorUv, "DeltaEngineLogo")
			};
			new ParticleEmitter(data, Point.Half);
		}

		[Test, CloseAfterFirstFrame]
		public void ParticlesUpdatingPosition()
		{
			CreateDataAndEmitter();
			emitter.Rotation = 0.0f;
			if (resolver.GetType() == typeof(MockResolver))
				AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(emitter.Center, emitter.particles[0].Position);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateParticleEmitterAddingDefaultComponents()
		{
			var emptyMaterial = new Material(Shader.Position2DColor, "");
			new ParticleEmitter(new ParticleEffectData { ParticleMaterial = emptyMaterial }, Point.Zero);
		}

		[Test]
		public void CreateEmitterAndKeepRunningWithAnimation()
		{
			var newEmitter = new ParticleEmitter(CreateDataAndEmitterWithAnimation("ImageAnimation"),
				Point.Half);
			newEmitter.Center = new Point(0.5f, 0.7f);
			new PerformanceTests.FpsDisplay();
		}

		private ParticleEffectData CreateDataAndEmitterWithAnimation(string contentName)
		{
			effectData = new ParticleEffectData
			{
				MaximumNumberOfParticles = 512,
				SpawnInterval = 0.1f,
				LifeTime = 5f,
				Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.10f)),
				Color = new RangeGraph<Color>(Color.White, Color.White),
				Force = new RangeGraph<Point>(new Point(0, 0.1f), new Point(0, 0.1f)),
				StartVelocity = new RangeGraph<Point>(new Point(0, -0.3f), new Point(0.05f, 0.01f)),
				ParticleMaterial = new Material(Shader.Position2DColorUv, contentName)
			};
			return effectData;
		}

		[Test]
		public void CreateEmitterAndKeepRunningWithSpriteSheetAnimation()
		{
			effectData = CreateDataAndEmitterWithAnimation("EarthSpriteSheet");
			emitter = new ParticleEmitter(effectData, Point.Half);
			emitter.Center = new Point(0.5f, 0.7f);
			new PerformanceTests.FpsDisplay();
		}

		[Test]
		public void CreatRotatedParticles()
		{
			effectData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			effectData.StartRotation = new ValueRange(45, 50);
			effectData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.05f));
			emitter = new ParticleEmitter(effectData, Point.Half);
			emitter.Center = new Point(0.5f, 0.7f);
			new PerformanceTests.FpsDisplay();
		}

		[Test]
		public void CreatRotatingParticles()
		{
			effectData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			effectData.StartRotation = new ValueRange(0, 45);
			effectData.RotationForce = new ValueRange(1, 1.2f);
			effectData.Size = new RangeGraph<Size>(new Size(0.05f), new Size(0.05f));
			emitter = new ParticleEmitter(effectData, Point.Half);
			emitter.Center = new Point(0.5f, 0.7f);
			new PerformanceTests.FpsDisplay();
		}

		[Test, CloseAfterFirstFrame]
		public void SpawnSingleBursts()
		{
			CreateDataAndEmitter(512, 0.01f, 5);
			emitter.SpawnBurst(20);
		}

		[Test, CloseAfterFirstFrame]
		public void DisposeEmitterAfterSetTime()
		{
			effectData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			effectData.SpawnInterval = 0.2f;
			effectData.LifeTime = 0.005f;
			emitter = new ParticleEmitter(effectData, new Point(0.5f, 0.5f));
			if (resolver.GetType() == typeof(MockResolver))
				AdvanceTimeAndUpdateEntities(0.2f);
			Assert.AreEqual(1, emitter.NumberOfActiveParticles);
			if (resolver.GetType() == typeof(MockResolver))
				AdvanceTimeAndUpdateEntities(0.09f);
			Assert.AreEqual(0, emitter.NumberOfActiveParticles);
		}

		[Test]
		public void LoadInAParticle()
		{
			ContentLoader.Load<ParticleEffectData>("TestParticle");
		}

		[Test]
		public void ParticleWithNoMaterialThrowsException()
		{
			effectData = CreateDataAndEmitterWithAnimation("DeltaEngineLogo");
			effectData.ParticleMaterial = null;
			Assert.Throws<ParticleEmitter.UnableToCreateWithoutMaterial>(
				() => new ParticleEmitter(effectData, new Point(0.5f, 0.5f)));
		}
	}
}