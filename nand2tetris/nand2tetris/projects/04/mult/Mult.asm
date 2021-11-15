// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
//
// This program only needs to handle arguments that satisfy
// R0 >= 0, R1 >= 0, and R0*R1 < 32768.
// R2 = R0 * R1
@R0 // D = R0
D=M
@ZERO 
D;JEQ // D == 0 JMP TO ZERO, because D = R0

@R1 // D = R1
D=M
@REPEAT // REPEAT = R1, because D = R1
M=D
@ZERO // D == 0 JMP TO ZERO, because D = R1
D;JEQ

@R2 // R2 = 0, cleans RAM[2] memory
M=0

(LOOP) // R0 AND R1, both greater than 0
    @R0 // D = R0
    D=M

    @R2 // R2 = R0 + R2
    M=D+M

    @REPEAT // REPEAT--
    M=M-1
    D=M // D = REPEAT
    @LOOP // REPEAT > 0 JMP TO LOOP
    D;JGT 
    @END // JMP TO END
    0;JMP
 
(ZERO) // R0 OR R1, one of them is equal to 0
    @R2
    M=0

(END) // INFINITY JUMP TO NOT ACCESS NOT UNAUTHORIZED AREA
    @END
    0; JMP