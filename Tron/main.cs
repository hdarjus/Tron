using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
public class Solver
{
    string[] dir = new string[] {"UP", "RIGHT", "DOWN", "LEFT"};
    int[] sides = new int[] {0, 29, 19, 0};
    int prev = 0;
    Random r = new Random();
    int[] possDirs = new int[4];
    int possDirsSize = 0;
    const int historySize = 200;
    int[] historyX = new int[historySize];
    int[] historyY = new int[historySize];
    int curr = 0;

    public Solver() {}

    public void Run() {
      string[] inputs;
      int? prevInt = null;

      // game loop
      while (true)
      {
          inputs = Console.ReadLine().Split(' ');
          int N = int.Parse(inputs[0]); // total number of players (2 to 4).
          int P = int.Parse(inputs[1]); // your player number (0 to 3).
          for (int i = 0; i < N; i++)
          {
              inputs = Console.ReadLine().Split(' ');
              int X0 = int.Parse(inputs[0]); // starting X coordinate of lightcycle (or -1)
              int Y0 = int.Parse(inputs[1]); // starting Y coordinate of lightcycle (or -1)
              int X1 = int.Parse(inputs[2]); // starting X coordinate of lightcycle (can be the same as X0 if you play before this player)
              int Y1 = int.Parse(inputs[3]); // starting Y coordinate of lightcycle (can be the same as Y0 if you play before this player)
              
              if (i == P) {
                  if (prevInt == null) {
                      prevInt = 0;
                      if (isFail(3, X1, Y1)) {
                          prev = 1;
                      } else {
                          prev = 3;
                      }
                  } else {
                      step(X1, Y1);
                  }
                  Console.WriteLine(dir[prev]);
              }

              historyX[curr] = X1;
              historyY[curr] = Y1;
              curr = (curr + 1) % historySize;
          }

          // Write an action using Console.WriteLine()
          // To debug: Console.Error.WriteLine("Debug messages...");

      }
    }
    
    bool isFail(int direction, int X, int Y) {
      if (direction%2 == 1) {
        if (X == sides[direction])
          return true;
      } else {
        if (Y == sides[direction])
          return true;
      }
      for (int i = 0; i < historySize; i++) {
        if (historyX[i] == X && historyY[i] == Y)
          return true;
      }
      return false;
    }

    void step (int X, int Y) {
      setPossibleDirections(X, Y);
      chooseDirection();
    }

    int preferredDir (int X, int Y) {
      if (2 * (prev%2==0 ? X : Y) < sides[(prev+1)%4] + sides[(prev+3)%4])
        return (prev%2)+1;
      else
        return ((prev%2)+3)%4;
    }

    void setPossibleDirections(int X, int Y) {
      possDirsSize = 0;
      if (!isFail(prev, X, Y)) {
        possDirs[possDirsSize] = prev;
        possDirsSize++;
      }
      int preferred = preferredDir(X, Y);
      preferred = (prev+1)%4;
      int notPreferred = (preferred+2)%4;
      if (!isFail(preferred, X, Y)) {
        possDirs[possDirsSize] = preferred;
        possDirsSize++;
      }
      if (!isFail(notPreferred, X, Y)) {
        possDirs[possDirsSize] = notPreferred;
        possDirsSize++;
      }
    }

    void chooseDirection () {
      int n = r.Next(1,101);
      switch (possDirsSize) {
        case 0:
          break;
        case 1:
          prev = possDirs[0];
          break;
        case 2:
          if (prev == possDirs[0]) {
            if (n > 30) {
              prev = possDirs[0];
            } else {
              prev = possDirs[1];
            }
          } else {
            if (n > 40)
              prev = possDirs[0];
            else
              prev = possDirs[1];
          }
          break;
        case 3:
          if (n > 20)
            prev = possDirs[0];
          else if (n > 8)
            prev = possDirs[1];
          else
            prev = possDirs[2];
          break;
        default:
          break;
      }
    }
}

class Player {
    static Solver s = new Solver();

    static void Main(string[] args)
    {
      s.Run();
    }
}

