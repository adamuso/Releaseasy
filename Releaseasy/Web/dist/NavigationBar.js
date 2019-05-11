import * as React from "react";
export class NavigationBar extends React.Component {
    render() {
        return React.createElement("div", null,
            React.createElement(NavigationMenu, null),
            React.createElement("div", null, "Releaseasy"),
            React.createElement("div", null, "log in"));
    }
}
class NavigationMenu extends React.Component {
    render() {
        const buttons = [
            "Test1",
            "Test2",
            "Test3"
        ];
        return React.createElement("div", null,
            React.createElement("div", null, "Logo"),
            React.createElement("div", null,
                React.createElement("ul", null, buttons.map((item) => React.createElement("li", null, item)))));
    }
}
//# sourceMappingURL=NavigationBar.js.map