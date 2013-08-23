﻿using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	internal class QuaternionTests
	{
		[Test]
		public void SetQuaternion()
		{
			var quaternion = new Quaternion(5.2f, 2.6f, 4.4f, 1.1f);
			Assert.AreEqual(5.2f, quaternion.X);
			Assert.AreEqual(2.6f, quaternion.Y);
			Assert.AreEqual(4.4f, quaternion.Z);
			Assert.AreEqual(1.1f, quaternion.W);
		}

		[Test]
		public void CreateQuaternionFromAxisAngle()
		{
			var axis = Vector.UnitY;
			const float Angle = 90.0f;
			var quaternion = Quaternion.FromAxisAngle(axis, Angle);
			var sinHalfAngle = MathExtensions.Sin(Angle * 0.5f);
			Assert.AreEqual(quaternion.X, axis.X * sinHalfAngle);
			Assert.AreEqual(quaternion.Y, axis.Y * sinHalfAngle);
			Assert.AreEqual(quaternion.Z, axis.Z * sinHalfAngle);
			Assert.AreEqual(quaternion.W, MathExtensions.Cos(Angle * 0.5f));
		}

		[Test]
		public void Length()
		{
			Assert.AreEqual(5.477f, new Quaternion(1, -2, 3, -4).Length, 0.001f);
		}

		[Test]
		public void Normalize()
		{
			var quaternion = new Quaternion(1, 3, 5, 7);
			var expected = new Quaternion(0.1091f, 0.3273f, 0.5455f, 0.7638f);
			Assert.AreEqual(expected, Quaternion.Normalize(quaternion));
		}

		[Test]
		public void MultiplyByFloat()
		{
			var quaternion = new Quaternion(1, 3, 5, 7);
			Assert.AreEqual(new Quaternion(-2, -6, -10, -14), quaternion * -2);
		}

		[Test]
		public void Lerp()
		{
			var rightOrientedQuat = Quaternion.FromAxisAngle(Vector.UnitY, 90.0f);
			var leftOrientedQuat = Quaternion.FromAxisAngle(Vector.UnitY, -90.0f);
			var result = rightOrientedQuat.Lerp(leftOrientedQuat, 0.5f);
			Assert.AreEqual(result, new Quaternion(0.0f, 0.0f, 0.0f, 0.7071f));
		}

		[Test]
		public void ConvertingQuaternionToMatrixAndBackLeavesItUnchanged()
		{
			var q1 = new Quaternion(1, 2, 3, 4);
			var q2 = new Quaternion(2, 0, 0, 1);
			var q3 = new Quaternion(0, 2, 0, 1);
			var q4 = new Quaternion(0, 0, 0, 1);
			Assert.AreEqual(q1, Quaternion.FromRotationMatrix(Matrix.FromQuaternion(q1)));
			Assert.AreEqual(q2, Quaternion.FromRotationMatrix(Matrix.FromQuaternion(q2)));
			Assert.AreEqual(q3, Quaternion.FromRotationMatrix(Matrix.FromQuaternion(q3)));
			Assert.AreEqual(q4, Quaternion.FromRotationMatrix(Matrix.FromQuaternion(q4)));
		}

		[Test]
		public void RotatingUnitXbyQuaternionMatchesRotatingItByMatrix()
		{
			var quaternion = Quaternion.FromAxisAngle(Vector.UnitY, 90.0f);
			var rotatedViaMatrix = Matrix.FromQuaternion(quaternion) * Vector.UnitX;
			var rotatedViaQuaternion = quaternion * Vector.UnitX;
			Assert.AreEqual(rotatedViaMatrix, rotatedViaQuaternion);
		}

		[Test]
		public void RotatingVectorbyQuaternionMatchesRotatingItByMatrix()
		{
			var axis = new Vector(4, 5, 6);
			axis.Normalize();
			var quaternion = Quaternion.FromAxisAngle(axis, 23.0f);
			var direction = new Vector(1, 2, 3);
			var rotatedViaMatrix = Matrix.FromQuaternion(quaternion) * direction;
			var rotatedViaQuaternion = quaternion * direction;
			Assert.AreEqual(rotatedViaMatrix, rotatedViaQuaternion);
		}

		[Test]
		public void MultiplyingTwoQuaternionsMatchesMultiplyingTwoMatrices()
		{
			var q1 = Quaternion.FromAxisAngle(Vector.Normalize(new Vector(1, 2, 3)), 40.0f);
			var q2 = Quaternion.FromAxisAngle(Vector.Normalize(new Vector(4, -5, 6)), -10.0f);
			var q3 = q1 * q2;
			var m1 = Matrix.FromQuaternion(q1);
			var m2 = Matrix.FromQuaternion(q2);
			var m3 = m1 * m2;
			Assert.AreEqual(q3, Quaternion.FromRotationMatrix(m3));
		}

		[Test]
		public void CheckVector()
		{
			Assert.AreEqual(new Vector(1, 2, 3), new Quaternion(1, 2, 3, 4).Vector);
		}

		[Test]
		public void String()
		{
			Assert.AreEqual("1, 2, 3, 4", new Quaternion(1, 2, 3, 4).ToString());
		}
	}
}