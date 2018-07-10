const template = `
<style>
    .sqaureDiv{
		height: 150px;
		width: 150px;
		background-color: red;
		margin: 5px;
	}
</style>
<div class="sqaureDiv"></div>
`;

customElements.define('square-component', class extends HTMLElement {

    constructor() {
        super();
		this.shadow = this.attachShadow({mode: 'open'});
        this.shadow.innerHTML = template;
    }

    connectedCallback() {

    }

    disconnectedCallback() {
		
    }

    static get observedAttributes() {
        return [];
    }

    attributeChangedCallback(attr, oldValue, newValue) {
        
    }
});