// See https://aka.ms/new-console-template for more information
var bingoNumbers = BingoFileParser.ParseBingoNumbers("BingoResults.txt");
var bingoBoards = BingoFileParser.ParseBingoBoards("BingoBoards.txt");

foreach (var bingoNumber in bingoNumbers)
{
    foreach (var bingoBoard in bingoBoards)
    {
        bingoBoard.MarkBingoNumber(bingoNumber);
        if (bingoBoard.HasLine)
        {
            var unmarkedNumbers = bingoBoard.GetUnmarkedNumbers();
            Console.WriteLine($"Winning score is: {unmarkedNumbers.Sum() * bingoNumber}");
            Console.ReadLine();
        }
    }
}

public static class BingoFileParser
{
    private const int BingoBoardSize = 5;
    public static List<int> ParseBingoNumbers(string fileName)
    {
        var input = File.ReadAllLines(fileName);
        var results = input.First().Split(',');
        return results.Select(int.Parse).ToList();
    }

    public static List<BingoBoard> ParseBingoBoards(string fileName)
    {
        var input = File.ReadAllText(fileName);
        var bingoBoards = new List<BingoBoard>();
        var boards = input.Split($"{Environment.NewLine}{Environment.NewLine}");
        foreach (var board in boards)
        {
            var bingoBoard = new BingoNumber[BingoBoardSize, BingoBoardSize];
            var boardLines = board.Split(Environment.NewLine);
            for (var i = 0; i < boardLines.Length; i++)
            {
                var columns = boardLines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var j = 0; j < columns.Length; j++)
                {
                    bingoBoard[i, j] = new BingoNumber
                    {
                        Number = int.Parse(columns[j]),
                        Marked = false
                    };
                }
            }
            bingoBoards.Add(new BingoBoard(bingoBoard));
        }

        return bingoBoards;
    }
}

public class BingoBoard
{
    private readonly BingoNumber[,] _bingoBoard;
    public BingoBoard(BingoNumber[,] bingoBoard)
    {
        _bingoBoard = bingoBoard;
    }

    public List<int> GetUnmarkedNumbers()
    {
        var unmarkedNumbers = new List<int>();
        for (var row = 0; row < _bingoBoard.GetLength(0); row++)
        {
            for (var col = 0; col < _bingoBoard.GetLength(1); col++)
            {
                var bingoBoardElement = _bingoBoard[row, col];
                if (!bingoBoardElement.Marked)
                    unmarkedNumbers.Add(bingoBoardElement.Number);
            }
        }
        return unmarkedNumbers;
    }

    public void MarkBingoNumber(int number)
    {
        for (var row = 0; row < _bingoBoard.GetLength(0); row++)
        {
            for (var col = 0; col < _bingoBoard.GetLength(1); col++)
            {
                var bingoBoardElement = _bingoBoard[row, col];
                if (bingoBoardElement.Number == number)
                    bingoBoardElement.Marked = true;
            }
        }
    }

    public bool HasLine => IsBoardWinningHorizontally() || IsBoardWinningVertically();

    private bool IsBoardWinningVertically()
    {
        for (var row = 0; row < _bingoBoard.GetLength(0); row++)
        {
            var isWinning = true;
            for (var col = 0; col < _bingoBoard.GetLength(1); col++)
            {
                var bingoBoardElement = _bingoBoard[row, col];
                if (bingoBoardElement.Marked)
                    continue;
                isWinning = false;
            }
            if (isWinning)
                return true;
        }
        return false;
    }

    private bool IsBoardWinningHorizontally()
    {
        for (var col = 0; col < _bingoBoard.GetLength(1); col++)
        {
            var isWinning = true;
            for (var row = 0; row < _bingoBoard.GetLength(0); row++)
            {
                var bingoBoardElement = _bingoBoard[row, col];
                if (bingoBoardElement.Marked)
                    continue;
                isWinning = false;
            }
            if (isWinning)
                return true;
        }
        return false;
    }
}

public class BingoNumber
{
    public int Number { get; set; }
    public bool Marked { get; set; }
}