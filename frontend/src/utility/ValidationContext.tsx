
export class ValidationContext {
    private hooks: (() => boolean)[] = [];

    attach(hook: () => boolean) {
        this.hooks.push(hook);
        return hook;
    }

    detach(hook: () => boolean) {
        const index = this.hooks.indexOf(hook);

        if (index >= 0) {
            this.hooks.splice(index, 1);
        }
    }

    validate(): boolean {
        for (let i = 0; i < this.hooks.length; i++) {
            if (!this.hooks[i]()) {
                return false;
            }
        }

        return true;
    }
}
