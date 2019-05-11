import { NavigationBar } from "./NavigationBar";
import * as React from "react";
import { HomePage } from "./pages/HomePage";
import { PageRouting, Route } from "./pages/PageRouting";
export class App extends React.Component {
    constructor(props) {
        super(props);
        if (document.location == null)
            throw new Error("The document does not provide a location");
        this.url = new URL(document.location.href);
    }
    render() {
        return React.createElement("div", null,
            React.createElement(NavigationBar, null),
            React.createElement("div", null,
                React.createElement(PageRouting, { url: this.url },
                    React.createElement(Route, { route: "/", page: HomePage }))));
    }
}
//# sourceMappingURL=App.js.map