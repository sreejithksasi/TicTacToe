using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Tests
{
    public class BoardTests
    {
        [Theory]
        [InlineData(-1)] // Test case 1: Negative number
        [InlineData(9)]  // Test case 2: Out of bounds for 3x3
        [InlineData(100)] // Test case 3: Way out of bounds
        public void ValidateUserInput_OutOfBounds_ReturnsFalse(int invalidInput)
        {
            // Arrange
            Board board = new(3);

            // Act
            bool isValid = board.ValidateUserInput(invalidInput);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void EvaluateWinCon_HorizontalWin_ReturnsWin()
        {
            // 1. ARRANGE: Set up the environment and state.
            // (If we had complex setup, we'd do it in the BoardTests constructor, 
            // but here instantiating the board is simple enough).
            Board board = new(3);
            board.UpdateBoard(0, 'X');
            board.UpdateBoard(1, 'X');
            board.UpdateBoard(2, 'X');

            // 2. ACT: Execute the specific method you are testing.
            WinStatus status = board.EvaluateWinCon(5);

            // 3. ASSERT: Verify the outcome is exactly what you expect.
            Assert.Equal(WinStatus.Win, status);
        }
    }
}
