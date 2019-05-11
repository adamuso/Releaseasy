import * as React from "react";
import { Page } from "./Page";

interface PageRoutingProps {
    url?: string | URL;
}

export class PageRouting extends React.Component<PageRoutingProps> {
    url : URL;

    constructor(props: PageRoutingProps) {
        super(props);

        if (typeof props.url === "string")
            this.url = new URL(props.url);
        else if(props.url)
            this.url = props.url;
        else
        {
            if(!document.location)
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
                    const props = child.props as RouteProps;

                    if (this.match(this.url, props.route))
                        return new props.page({ params: { params } });
                }
            }
        }

        return null;
    }

    private match(url: URL, route: string): boolean {


        return true;
    }
}

interface RouteProps {
    route: string;
    page: new (props: any) => Page;
}

export class Route extends React.Component<RouteProps> {

}