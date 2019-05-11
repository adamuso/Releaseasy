import * as React from "react";

export class NavigationBar extends React.Component {
    render(): React.ReactNode {
        return <div>
            <NavigationMenu/>
            <div>
                Releaseasy
            </div>
            <div>
                log in
            </div>
        </div>;
    }
}

class NavigationMenu extends React.Component {
    render() {
        const buttons: string[] = [
            "Test1",
            "Test2",
            "Test3"
        ]

        return <div>
            <div>Logo</div>
            <div>
                <ul>
                    {buttons.map((item) => <li>{item}</li>)}
                </ul>
            </div>
        </div>;
    }
}