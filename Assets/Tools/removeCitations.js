// This is just a really simple tool to run in the console to remove the citations I've put in the facts, since I don't want them in the game

const removeCitations = text => {
    let inCitation = false;
    let newText = "";
    for (let char of text) {
        if (char == "[") {
            inCitation = true;
            continue;
        }
        if (inCitation) {
            if (char == "]") {
                inCitation = false;
                continue;
            }
        }
        else {
            newText += char;
        }
    }

    console.log(newText);
};