import { Page } from "./Page";
import * as React from "react";
import { User } from "../backend/User";
import { Application } from "../main";

interface HomePageState {
    username: string,
    password: string
}

export class HomePage extends Page<HomePageState> {
    private animatedTextRef = React.createRef<HTMLDivElement>();
    private textAnimationTimeout: number = 0;
    private animatedTexts = [
        "deploy",
        "research",
        "create",
        "release",
        "make",
        "think"
    ];

    constructor(props: {}) {
        super(props);

        this.state = {
            username: "",
            password: ""
        };
    }

    render() {
        return <div className="home-page">
            <div style={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center"
            }}>
                <div className="welcome-text">
                    <div>
                        Let's
                    </div>
                    <div className="welcome-text-animated" ref={this.animatedTextRef}>
                    </div>
                </div>
                <div className="sign-in" onClick={() => this.loginOnClick()}>
                    <div>
                        sign in
                    </div>
                </div>
            </div>
        </div>;
    }

    componentDidMount() {
        let currentAnimatedText = 0;
        let currentAnimatedTextLetter = 0;
        let remove = false;

        const animationFunc = () => {
            if (remove) {
                this.animatedTextRef.current!.textContent = this.animatedTextRef.current!.textContent!.substring(0, this.animatedTextRef.current!.textContent!.length - 1);

                if (this.animatedTextRef.current!.textContent.length <= 0) {
                    remove = false;
                    currentAnimatedTextLetter = 0;
                    currentAnimatedText = (currentAnimatedText + 1) % this.animatedTexts.length;
                }

                this.textAnimationTimeout = setTimeout(animationFunc, 50);
                return;
            }

            this.animatedTextRef.current!.textContent = this.animatedTexts[currentAnimatedText][this.animatedTexts[currentAnimatedText].length - currentAnimatedTextLetter - 1] + this.animatedTextRef.current!.textContent;

            currentAnimatedTextLetter++;

            if (currentAnimatedTextLetter === this.animatedTexts[currentAnimatedText].length) {
                this.textAnimationTimeout = setTimeout(animationFunc, 1000);
                remove = true;
                return;
            }

            this.textAnimationTimeout = setTimeout(animationFunc, 100);
        };

        this.textAnimationTimeout = setTimeout(animationFunc, 100);
    }

    componentWillUnmount() {
        clearTimeout(this.textAnimationTimeout);
    }

    async loginOnClick() {
        Application.reactApp.changePage("Login");
    }
}