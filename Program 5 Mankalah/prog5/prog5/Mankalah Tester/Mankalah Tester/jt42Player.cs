/////////////////////////////////////////////////
///
/// Jacob Tocila Mankalah CS 212, December 2, 2023
/// Total work time: 1 hour Wednesday, 3 hours Thursday, 2 hours Friday afternoon, 1.5 hours Friday evening, 4.5 hours Saturday evening
///
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace Mankalah
{
    /*****************************************************************
     * A Mankalah player.  This is the base class for players.
     * You'll derive a player from this base. Be sure your player
     * works correctly both as TOP and as BOTTOM.
     *****************************************************************/
    public class jt42Player : Player
    {
        private String name;
        private Position position;
        private int timePerMove;    // time allowed per move in msec
        private int depth;

        public jt42Player(Position pos, int timeLimit) : base(pos, "Jacob Tocila (AI)", timeLimit) { }

        /*
         * Evaluate: return a number saying how much we like this board. 
         * TOP is MAX, so positive scores should be better for TOP.
         * This default just counts the score so far. Override to improve!
         */
        public override int evaluate(Board board)
        {
            // The goals are at index 6 (bottom player) and index 13 (top player)
            int score = board.stonesAt(13) - board.stonesAt(6);

            int totalStones = 0;
            int totalGoAgains = 0;
            int totalCaptures = 0;


            // Loop through the holes in the top row (index 7 to 12)
            for (int i = 7; i <= 12; i++)
            {
                int priority = 0;

                // Calculate the target pit for possible captures
                //int targetPit = (i + board.stonesAt(i)) % 6;
                //int targetStonesAt = board.stonesAt(targetPit);
                int targetPit = board.stonesAt(i) % (13 - i);
                int targetStonesAt = board.stonesAt(targetPit + 7);


                // If it's my turn and I'm on bottom
                if (board.whoseMove() == Position.Bottom)
                {
                    totalStones = totalStones - board.stonesAt(i);

                    // Check for go again moves aka having the last stone land in the goal pit
                    // I needed help doing the math for these
                    if ((board.stonesAt(i) - (13 - i) == 0) || (board.stonesAt(i) - (13 - i) == 13))
                    {
                        totalGoAgains = totalGoAgains - (priority + 2);
                    }

                    // Check for captures aka the hole you just landed in was empty and the hole across the board vertically has stones in it
                    // I also needed help doing the math for these
                    if (targetStonesAt == 0 && board.stonesAt(i) == (13 - i + targetPit + 7))
                    {
                        totalCaptures = totalCaptures + (board.stonesAt(i) + board.stonesAt(12 - targetPit));

                    }

                }

                // If it's my turn and I'm on top
                if (board.whoseMove() == Position.Top)
                {
                    totalStones = totalStones + board.stonesAt(i);

                    // Check for go again moves aka having the last stone land in the goal pit
                    // I needed help doing the math for these
                    if ((board.stonesAt(i) - (13 - i) == 0) || (board.stonesAt(i) - (13 - i) == 13))
                    {
                        totalGoAgains = totalGoAgains + (priority + 2);
                    }

                    // Check for captures aka the hole you just landed in was empty and the hole across the board vertically has stones in it
                    // I also needed help doing the math for these
                    if (targetStonesAt == 0 && board.stonesAt(i) == (13 - i + targetPit + 7))
                    {
                        totalCaptures = totalCaptures - (board.stonesAt(i) + board.stonesAt(12 - targetPit));
                    }
                }
                priority++;
            }

            // Loop through the holes in the bottom row (index 0 to 5)
            for (int i = 0; i <= 5; i++)
            {
                int priority = 0;

                // Calculate the target pit for possible captures
                int targetPit = (board.stonesAt(i)) % (13 - 1);
                int targetStonesAt = board.stonesAt(targetPit);

                // If it's my turn and I'm on bottom
                if (board.whoseMove() == Position.Bottom)
                {
                    totalStones = totalStones + board.stonesAt(i);

                    // Check for go again moves aka having the last stone land in the goal pit
                    // I needed help doing the math for these
                    if ((board.stonesAt(i) - (6 - i) == 0) || (board.stonesAt(i) - (6 - i) == 13))
                    {
                        totalGoAgains = totalGoAgains - (priority + 2);
                    }

                    // Check for captures aka the hole you just landed in was empty and the hole across the board vertically has stones in it
                    // I also needed help doing the math for these
                    if (targetStonesAt == 0 && board.stonesAt(i) == (13 - i + targetPit))
                    {
                        totalCaptures = totalCaptures - (board.stonesAt(i) + board.stonesAt(12 - targetPit));
                    }
                }

                // If it's my turn and I'm on top
                if (board.whoseMove() == Position.Top)
                {
                    totalStones = totalStones - board.stonesAt(i);

                    // Check for go again moves aka having the last stone land in the goal pit
                    // I needed help doing the math for these
                    if ((board.stonesAt(i) - (6 - i) == 0) || (board.stonesAt(i) - (6 - i) == 13))
                    {
                        totalGoAgains = totalGoAgains + (priority + 2);
                    }

                    // Check for captures aka the hole you just landed in was empty and the hole across the board vertically has stones in it
                    // I also needed help doing the math for these
                    if (targetStonesAt == 0 && board.stonesAt(i) == (13 - i + targetPit))
                    {
                        totalCaptures = totalCaptures + (board.stonesAt(i) + board.stonesAt(12 - targetPit));
                    }
                }
                priority++;
            }

            //Updates the final score with total stones, total captures, and total go again moves
            score = score + totalStones + totalCaptures + totalGoAgains;
            return score;
        }

        public String getName()
        {
            return name;
        }

        public int getTimePerMove()
        {
            return timePerMove;
        }

        public String getImage()
        {
            return "happy.jpg";
        }

        // Override with your own choosemove function
        public override int chooseMove(Board board)
        {
            // Create a stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int iteration = 1;
            Result bestMove = new Result(0, 0, false);

            try
            {

                while (!bestMove.isEndGame())
                {
                    bestMove = minimaxVal(board, iteration++, stopwatch, int.MinValue, int.MaxValue);
                    //bestMove = minimaxVal(board, iteration++, stopwatch);
                }
            }
            catch (MoveTimedOutException)
            {
                // put something here later
            }
            stopwatch.Stop();

            // Return the best move minimax was able to get within the time limit
            return bestMove.getMove();
        }

        /*
         * This psuedocode is from "Minimax: How Computers Play Games" by Spanning Tree on YouTube
         * https://www.youtube.com/watch?v=SLgZhpDsrfc
         * 
        Minimax(s):
        
            if Terminal(s):
                return Value(s)
            
            if Player(s) == MAX:
                value = -infinity
                for a in Action(s):
                    value = Max(value, Minimax(Result(s, a)))
                return value

            if Player(s) == MIN:
                value = infinity
                for a in Action(s):
                    value = Min(value, Minimax(Result(s, a)))
                return value
         */


        private Result minimaxVal(Board board, int depth, Stopwatch stopwatch, int alpha, int beta)
        {
            // See if there is still time remaining (then can do more searches)
            if (stopwatch.ElapsedMilliseconds > getTimePerMove())
            {
                throw new MoveTimedOutException();
            }

            int bestMove = 0;
            int bestValue = 0;
            // temporarily setting bestValue = 0
            bool gameCompleted = false;

            // Check if game is over
            if (board.gameOver() || depth == 0)
            {
                return new Result(0, evaluate(board), board.gameOver());
            }

            Position currentPlayer = board.whoseMove();

            // Top player wants max valued points
            if (currentPlayer == Position.Top)
            {
                Console.WriteLine("top");
                bestValue = Int32.MinValue;
                // Look at possible moves for player while in top position
                for (int move = 7; move <= 12; move++)
                {
                    if (board.legalMove(move))
                    {
                        Board nextBoard = new Board(board);
                        nextBoard.makeMove(move, false);

                        Result result = minimaxVal(nextBoard, depth - 1, stopwatch, alpha, beta);
                        //Console.WriteLine("jfhjksd");
                        if (result.getScore() > bestValue)
                        {
                            bestValue = result.getScore();
                            bestMove = move;
                            gameCompleted = result.isEndGame();
                        }
                        if (bestValue > alpha)
                        {
                            alpha = bestValue;
                        }
                    }
                }
            }

            // Bottom player wants min valued points
            if (currentPlayer == Position.Bottom)
            {
                bestValue = Int32.MaxValue;
                //Console.WriteLine("bottom");
                // Look at possible moves for player while in bottom position
                for (int move = 0; move <= 5; move++)
                {
                    //Console.WriteLine("in for loop");
                    if (board.legalMove(move))
                    {

                        Board nextBoard = new Board(board);
                        nextBoard.makeMove(move, false);

                        Result result = minimaxVal(nextBoard, depth - 1, stopwatch, alpha, beta);
                        //Console.WriteLine("Making move at index " + result.getScore());

                        if (result.getScore() < bestValue)
                        {
                            bestValue = result.getScore();
                            bestMove = move;
                            gameCompleted = result.isEndGame();
                        }
                        if (bestValue < beta)
                        {
                            beta = bestValue;
                        }
                    }
                }
            }
            //Console.WriteLine(bestValue); 
            return new Result(bestMove, bestValue, gameCompleted);
        }


        // Override with your own personalized gloat.
        public override String gloat()
        {
            return "You were an admirable adversary.";
        }
    }

    class MoveTimedOutException : Exception { }

    // This represents the game via the result of a game move / organizing the move itself, player score, and if the game is over or not.
    class Result
    {
        private int Move;
        private int Score;
        private bool GameState;
        public Result(int move, int score, bool state)
        {
            Move = move;
            Score = score;
            GameState = state;
        }

        public int getMove()
        {
            return Move;
        }

        public int getScore()
        {
            return Score;
        }

        public bool isEndGame()
        {
            return GameState;
        }
    }
}

// test 500
