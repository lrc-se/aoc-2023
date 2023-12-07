Advent of Code 2023
===================

Solutions for the 2023 edition of the [Cygnified Advent of Code](https://aoc.cygni.se/).

This time I'll concentrate on bleeding-edge C#.NET, trying to squeeze out as much performance and DX as possible from the language and framework.


Example
-------

The repo includes a base example in C# using .NET 8 with no external dependencies. The idea is once more to reuse common infrastructure code between solutions, only modifying functions in the puzzle file (and adding more files when necessary). [Last year's](https://github.com/lrc-se/aoc-2022) common code has been simplified further and more things moved to the run script. The *Dockerfile* also performs a native AOT compilation, further enhancing performance characteristics (hopefully).

The environment variable `part` is recognized as follows:

- `part1`: only runs part one
- `part2`: only runs part two

Any other value will abort the execution.

### Run script

The base example also include a shell script *run.sh*, which will time the execution of the solution. It has the following syntax:

`run.sh part [mode]`

- `part`: which part to run (sets the `part` environment variable accordingly)
- `mode`:
  - `test`: activates test mode
  - `rel`: builds the solution in release mode
  - `test-rel`: combines `test` and `rel`

If the `mode` argument is omitted, the puzzle will be run in normal mode without release optimizations. Input is read from *input.txt* in normal mode, and from *input-test.txt* in test mode.


Exercises
---------

Also this time I've made some re-implementations of previous puzzles:

### 2022: Day 10

[Originally solved](https://github.com/lrc-se/aoc-2022/blob/main/day10/Puzzle.fs) in F#, recent functional and syntactical enhancements to C# made for a pretty close match as a direct port. I've also added a stateful variant, and a procedural version employing an interator while still benefitting from pattern matching.

### 2022: Day 23

This is basically a port of my [original Nim solution](https://github.com/lrc-se/aoc-2022/blob/b89e50a699b0bf3adad05f0b6dcca464efd6ccbb/day23/puzzle.nim), leveraging collection expressions and type aliases as well as some fresh optimization techniques. The result actually outperforms the Nim version, at least for part 2.


Puzzles
-------

### Day 1

Well, this was a considerably more tricky start than usual, and I stumbled on regex overlap handling. Also note that I have no mechanism for variable test input, so the current setup will give the wrong answer for part 2's test data.

### Day 2

Today was just about parsing, which was handled without regexes this time.

### Day 3

Regex, generics and immutable data structures, with a mix between functional and imperative.

### Day 4

Regexes, bit shifts and LINQ made short work of this.

### Day 5

Oh boy, exponential iteration explosion in part 2 already... I had several false starts (and true stops) trying to calculate reverse ranges, but then went for a brute force solution which simply tried to hit the seed ranges starting from increasing location values. That worked but took a couple of seconds to run, so I then made another attempt using ranges, only in the forward direction instead, which turned out to work just fine and was also almost instantaneous. But oh, how I miss a pipeline operator in C#...

### Day 6

Just a straight-up loop this time, nothing fancy; 'twas fast enough throughout. Very varying difficulty levels early on this year!
*__Update:__ Adopted a mathematical perspective, which reduced execution times considerably for part 2.*

### Day 7

Solved part 2 with an explicit brute-force combination search, which I then made more efficient with a cache. There are probably better ways, but this is quite fast already.
