using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Tests
{
    public class MoveTests
    {
        [Fact]
        public async Task MakeMove_ValidInput_UpdatesBoard()
        {
            // Arrange
            Board board = new(3);

            // We create a Mock of the interface, NOT the concrete ConsoleInputProvider
            var mockInputProvider = new Mock<IInputProvider>();

            // We instruct the mock: "When ReadInput() is called, immediately resolve the Task with the number 4."
            mockInputProvider.Setup(x => x.ReadInput()).ReturnsAsync(4);

            // We pass the mock object into our Move class
            Move playerMove = new(board, 'X', mockInputProvider.Object);

            // Act
            bool result = await playerMove.MakeMove();

            // Assert
            Assert.True(result);
        }
    }
}
