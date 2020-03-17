import * as React from "react";
import { Page } from "./Page";

interface PageRoutingProps {
    url?: string | URL;
    state?: object;
}

export class PageRouting extends React.Component<PageRoutingProps> {
    constructor(props: PageRoutingProps) {
        super(props);
    }

    render() {

        let url: URL;

        if (typeof this.props.url === "string")
            url = new URL(this.props.url);
        else if (this.props.url)
            url = this.props.url;
        else {
            if (!document.location)
                throw "Document location is missing";

            url = new URL(document.location.href);
        }

        const params = {};
        const state = typeof this.props.state === "object" ? this.props.state : {};

        for (const entry of url.searchParams.entries()) {
            params[entry[0]] = entry[1];   
        }

        const children = this.props.children;

        if (children instanceof Array) {
            for (const child of children) {
                if (!child || typeof child !== "object")
                    continue;

                if ("type" in child) {
                    const props = child.props as RouteProps;

                    if (this.match(url, props.route)) {
                        return React.createElement(props.page, { params: { ...params, ...state } });
                    }
                }
            }
        }
        else if (children && typeof children === "object" && "type" in children) {
            const props = children.props as RouteProps;

            if (this.match(url, props.route))
                return React.createElement(props.page, { params: { ...params, ...state } });
        }

        return null;
    }

    private match(url: URL, route: string): boolean {

        if (url.pathname.toLowerCase() === route.toLowerCase())
            return true;

        return false;
    }
}

interface RouteProps {
    route: string;
    page: new (props: any) => Page;
}

export class Route extends React.Component<RouteProps> {

}