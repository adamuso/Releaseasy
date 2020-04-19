import { Component } from "react";

type BindingMode = undefined | "one-way";

export abstract class BindableComponent<TProps, TState> extends Component<TProps, TState> {
    protected binder = <TKey extends keyof TState>(state: TKey, mode?: BindingMode) => this.bind(state, mode);

    protected bind<TKey extends keyof TState, TValue = Required<TState>[TKey]>(state: TKey, mode?: BindingMode) {
        return (value?: TValue) => {
            if (value !== undefined && mode !== "one-way") {
                this.setState({ [state]: value } as {});
                return value;
            }

            return this.state[state];
        }
    }
}