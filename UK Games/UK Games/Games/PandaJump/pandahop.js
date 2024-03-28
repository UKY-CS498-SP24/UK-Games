//Playground
let board;
let boardWidth = 400;
let boardHeight = 600;
let context;

//Panda
let pandaWidth = 46;
let pandaHeight = 46;
let pandaX = boardWidth/2 -pandaWidth/2;
let pandaY = boardHeight*7/8 - pandaHeight;
let pandaLeft;
let pandaRight;

let panda = {
    img : null,
    x: pandaX,
    y: pandaY,
    width : pandaWidth,
    height : pandaHeight
}

//platforms
let platformArray = [];
let platformWidth = 60;
let platformHeight = 40;
let platformImg;

//scores
let score = 0;
let maxScore = 0;
let gameOver = false;


//mechanics
let velocityX = 0; // movement speed
let velocityY = 0; // jump speed
let initialVelocityY = -8; // y velocity
let gravity = .4; // time to jump

window.onload = function(){
    board = document.getElementById("board");
    board.height = boardHeight;
    board.width = boardWidth;
    context = board.getContext("2d"); // draws the playground

    // draw panda
     pandaRight = new Image();
     pandaRight.src = "./panda-right.png";
     panda.img = pandaRight;
     pandaRight.onload = function(){
        context.drawImage(panda.img, panda.x, panda.y, panda.width, panda.height)
    }
    //context.fillStyle = "green";
    //context.fillRect(panda.x, panda.y, panda.width, panda.height);

    pandaLeft = new Image();
    pandaLeft.src = "./panda-left.png";

    platformImg = new Image();
    platformImg.src = "./platform.png";

    velocityY = initialVelocityY;
    placePlatforms();
 
    requestAnimationFrame(update);
    document.addEventListener("keydown", movePanda);

    
}

function update(){
    requestAnimationFrame(update);

    if (gameOver) {
        return;
    }

    context.clearRect(0, 0, board.width, board.height);
    //panda
    panda.x += velocityX;
    if(panda.x > boardWidth){
        panda.x = 0;
    } else if( panda.x + panda.width < 0){
        panda.x = boardWidth;
    }

    velocityY += gravity;
    panda.y += velocityY;
    if (panda.y > board.height) {
        gameOver = true;
    }
    context.drawImage(panda.img, panda.x, panda.y, panda.width, panda.height);

    
     //platforms
     for (let i = 0; i < platformArray.length; i++) {
        let platform = platformArray[i];
        if (velocityY < 0 && panda.y < boardHeight*3/4) {
            platform.y -= initialVelocityY; //slide platform down
        }
        if (detectCollision(panda, platform) && velocityY >= 0) {
            velocityY = initialVelocityY; //jump
        }
        context.drawImage(platform.img, platform.x, platform.y, platform.width, platform.height);
    }

    // clear platforms and add new platform
    while (platformArray.length > 0 && platformArray[0].y >= boardHeight) {
        platformArray.shift(); //removes first element from the array
        newPlatform(); //replace with new platform on top
    }

    //score
    updateScore();
    context.fillStyle = "black";
    context.font = "16px sans-serif";
    context.fillText(score, 5, 20);

    if (gameOver) {
        context.fillText("Game Over: Press 'Space' to Restart", boardWidth/7, boardHeight*7/8);
    }

}

function newPlatform() {
    let randomX = Math.floor(Math.random() * boardWidth*3/4); 
    let platform = {
        img : platformImg,
        x : randomX,
        y : -platformHeight,
        width : platformWidth,
        height : platformHeight
    }

    platformArray.push(platform);
}

function placePlatforms() {
    platformArray = [];

    //start
    let platform = {
        img : platformImg,
        x : boardWidth/2,
        y : boardHeight - 50,
        width : platformWidth,
        height : platformHeight
    }

    platformArray.push(platform);

    for (let i = 0; i < 6; i++) {
        let randomX = Math.floor(Math.random() * boardWidth*3/4); //(0-1) * boardWidth*3/4
        let platform = {
            img : platformImg,
            x : randomX,
            y : boardHeight - 75*i - 150,
            width : platformWidth,
            height : platformHeight
        }
    
        platformArray.push(platform);
    }
}

function movePanda(k) {
    if(k.code == "ArrowRight" || k.code == "KeyD"){ //move right
        velocityX = 4;
        panda.img = pandaRight;
    } else if (k.code == "ArrowLeft" || k.code == "KeyA"){ //move left
        velocityX = -4;
        panda.img = pandaLeft;
    }  else if (k.code == "Space" && gameOver) {
        //reset
        panda = {
            img : pandaRight,
            x : pandaX,
            y : pandaY,
            width : pandaWidth,
            height : pandaHeight
        }

        velocityX = 0;
        velocityY = initialVelocityY;
        score = 0;
        maxScore = 0;
        gameOver = false;
        placePlatforms();
    }

}

function detectCollision(a, b) {
    return a.x < b.x + b.width &&   //a's top left corner doesn't reach b's top right corner
           a.x + a.width > b.x &&   //a's top right corner passes b's top left corner
           a.y < b.y + b.height &&  //a's top left corner doesn't reach b's bottom left corner
           a.y + a.height > b.y;    //a's bottom left corner passes b's top left corner
}

function updateScore() {
    let points = Math.floor(50*Math.random()); //(0-1) *50 --> (0-50)
    if (velocityY < 0) { //negative going up
        maxScore += points;
        if (score < maxScore) {
            score = maxScore;
        }
    }
    else if (velocityY >= 0) {
        maxScore -= points;
    }
}