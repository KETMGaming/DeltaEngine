using DeltaEngine.Commands;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class Game
	{
		public Game(Window window)
		{
			new RelativeScreenSpace(window);
			gridSize = 25;
			blockSize = 1.0f / gridSize;
			this.window = window;
			menu = new Menu();
			menu.InitGame += StartGame;
			menu.Quit += window.CloseAfterFrame;
		}

		public void StartGame()
		{
			menu.Hide();
			SetupPlayArea();
			SetInput();
			InitializeSnake();
			SpawnFirstChunk();
		}

		private readonly int gridSize;
		private readonly float blockSize;
		private readonly Window window;
		private readonly Menu menu;

		private void SetupPlayArea()
		{
			window.Title = "Snake - Let's go";
			window.BackgroundColor = menu.gameColors [1];
			new FilledRect(CalculateBackgroundDrawArea(), menu.gameColors [0]);
		}

		private Rectangle CalculateBackgroundDrawArea()
		{
			return new Rectangle(blockSize, blockSize, blockSize * (gridSize - 2), blockSize * 
				(gridSize - 2));
		}

		private void SetInput()
		{
			new Command(MoveLeft).Add(new KeyTrigger(Key.CursorLeft));
			new Command(MoveRight).Add(new KeyTrigger(Key.CursorRight));
			new Command(MoveUp).Add(new KeyTrigger(Key.CursorUp));
			new Command(MoveDown).Add(new KeyTrigger(Key.CursorDown));
			new Command(MoveAccordingToTouchPosition).Add(new TouchPressTrigger()).Add(new 
				MouseButtonTrigger());
		}

		public void MoveLeft()
		{
			if (GetDirection().X > 0)
				return;

			snakeBody.Direction = new Vector2D(-blockSize, 0);
		}

		private Vector2D GetDirection()
		{
			if (snakeBody.BodyParts.Count == 0)
				return Vector2D.Zero;

			var snakeHead = snakeBody.BodyParts [0];
			var partNextToSnakeHead = snakeBody.BodyParts [1];
			var direction = new Vector2D(snakeHead.DrawArea.Left - partNextToSnakeHead.DrawArea.Left, 
				snakeHead.DrawArea.Top - partNextToSnakeHead.DrawArea.Top);
			return direction;
		}

		public void MoveRight()
		{
			if (GetDirection().X < 0)
				return;

			snakeBody.Direction = new Vector2D(blockSize, 0);
		}

		public void MoveUp()
		{
			if (GetDirection().Y > 0)
				return;

			snakeBody.Direction = new Vector2D(0, -blockSize);
		}

		public void MoveDown()
		{
			if (GetDirection().Y < 0)
				return;

			snakeBody.Direction = new Vector2D(0, blockSize);
		}

		private void MoveAccordingToTouchPosition(Vector2D position)
		{
			var comparison = snakeBody.HeadPosition;
			CheckTouchHorizontal(position, comparison);
			CheckTouchVertical(position, comparison);
		}

		private void CheckTouchVertical(Vector2D position, Vector2D comparison)
		{
			if (GetDirection().X != 0)
			{
				var deltaY = position.Y - comparison.Y;
				if (deltaY > 0)
					MoveDown();

				if (deltaY < 0)
					MoveUp();
			}
		}

		private void CheckTouchHorizontal(Vector2D position, Vector2D comparison)
		{
			if (GetDirection().Y != 0)
			{
				var deltaX = position.X - comparison.X;
				if (deltaX > 0)
					MoveRight();

				if (deltaX < 0)
					MoveLeft();
			}
		}

		private void InitializeSnake()
		{
			Snake = new $safeprojectname$(gridSize, menu.gameColors [2]);
			snakeBody = Snake.Get<Body>();
			AddEventListeners();
		}

		private Body snakeBody;

		public $safeprojectname$ Snake
		{
			get;
			private set;
		}

		private void AddEventListeners()
		{
			snakeBody.DetectSnakeCollisionWithChunk += SnakeCollisionWithChunk;
			snakeBody.SnakeCollidesWithBorderOrItself += Reset;
		}

		private void SnakeCollisionWithChunk(Vector2D trailingVector)
		{
			if (Chunk.TopLeft.IsNearlyEqual(snakeBody.BodyParts [0].TopLeft))
			{
				Chunk.SpawnAtRandomLocation();
				GrowSnakeInSize(trailingVector);
			}
		}

		private void GrowSnakeInSize(Vector2D trailingVector)
		{
			var snakeBodyParts = snakeBody.BodyParts;
			var tail = snakeBodyParts [snakeBodyParts.Count - 1].DrawArea.TopLeft;
			var newBodyPart = new FilledRect(CalculateTrailDrawArea(trailingVector, tail), 
				menu.gameColors [2]);
			snakeBodyParts.Add(newBodyPart);
			window.Title = "Snake - Length: " + snakeBodyParts.Count;
		}

		private Rectangle CalculateTrailDrawArea(Vector2D trailingVector, Vector2D tail)
		{
			return new Rectangle(new Vector2D(tail.X + trailingVector.X, tail.Y + trailingVector.Y), 
				new Size(blockSize));
		}

		public void Reset()
		{
			RemoveEventListeners();
			Snake.Dispose();
			DisplayGameOverMessage();
		}

		private void DisplayGameOverMessage()
		{
			Chunk.IsActive = false;
			var fontGameOverText = ContentLoader.Load<Font>("Tahoma30");
			var fontReplayText = ContentLoader.Load<Font>("Verdana12");
			gameOverMsg = new FontText(fontGameOverText, "Game Over", 
				Rectangle.FromCenter(Vector2D.Half, new Size(0.6f, 0.3f))) {
				Color = menu.gameColors[1],
				RenderLayer = 3
			};
			restartMsg = new FontText(fontReplayText, "Do you want to continue (Y/N)", 
				Rectangle.FromCenter(new Vector2D(0.5f, 0.7f), new Size(0.6f, 0.3f))) {
				Color = menu.gameColors[2],
				RenderLayer = 3
			};
			yesCommand = new Command(RestartGame);
			yesCommand.Add(new KeyTrigger(Key.Y));
			noCommand = new Command(CloseGame);
			noCommand.Add(new KeyTrigger(Key.N));
		}

		private Command yesCommand;
		private Command noCommand;
		private FontText gameOverMsg;
		private FontText restartMsg;

		private void RestartGame()
		{
			yesCommand.IsActive = false;
			noCommand.IsActive = false;
			gameOverMsg.IsActive = false;
			restartMsg.IsActive = false;
			InitializeSnake();
			SpawnFirstChunk();
		}

		private void CloseGame()
		{
			window.CloseAfterFrame();
		}

		private void SpawnFirstChunk()
		{
			Chunk = new Chunk(gridSize, blockSize, menu.gameColors [3]);
			RespawnChunk();
		}

		public Chunk Chunk
		{
			get;
			private set;
		}

		public void RespawnChunk()
		{
			while (Chunk.IsCollidingWithSnake(snakeBody.BodyParts))
				Chunk.SpawnAtRandomLocation();
		}

		private void RemoveEventListeners()
		{
			snakeBody.DetectSnakeCollisionWithChunk -= SnakeCollisionWithChunk;
			snakeBody.SnakeCollidesWithBorderOrItself -= Reset;
		}
	}
}