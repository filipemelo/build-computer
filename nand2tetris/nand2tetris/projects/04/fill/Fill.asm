// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.
(loop)
@SCREEN
D=A
@address // Saves the address from screen (16384)
M=D 

@8192
D=A
@pixels
M=D

(screen_loop)
    @KBD
    D=M
    @if_black
    D;JGT
    D=0 // else
    (end_if_black)
    @address // load screen memory
    A=M
    M=D

    @address //set the next screen memory 
    M=M+1

    @pixels
    M=M-1
    D=M
    @screen_loop
    D, JGT

@loop
0, JMP

(if_black)
    @color
    D=-1
    @end_if_black
    0;JMP
