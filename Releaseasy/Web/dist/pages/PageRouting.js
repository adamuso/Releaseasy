import * as React from "react";
export class PageRouting extends React.Component {
    constructor(props) {
        super(props);
        if (typeof props.url === "string")
            this.url = new URL(props.url);
        else if (props.url)
            this.url = props.url;
        else {
            if (!document.location)
                throw "Document location is missing";
            this.url = new URL(document.location.href);
        }
    }
    render() {
        const params = {};
        for (const entry of this.url.searchParams.entries()) {
            params[entry[0]] = entry[1];
        }
        const children = this.props.children;
        if (children instanceof Array) {
            for (const child of children) {
                if (!child || typeof child !== "object")
                    continue;
                if ("type" in child) {
                    const props = child.props;
                    if (this.match(this.url, props.route))
                        return new props.page({ params: { params } });
                }
            }
        }
        return null;
    }
    match(url, route) {
        return true;
    }
}
export class Route extends React.Component {
}
//# sourceMappingURL=PageRouting.js.map