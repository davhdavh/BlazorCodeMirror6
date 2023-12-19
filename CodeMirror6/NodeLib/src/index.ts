import {basicSetup} from "codemirror"
import {EditorView, keymap, placeholder, KeyBinding} from "@codemirror/view"
import {EditorState, Compartment} from "@codemirror/state"
import {indentWithTab} from "@codemirror/commands"
import { indentUnit } from "@codemirror/language"
import {cpp} from "@codemirror/lang-cpp"
import {css} from "@codemirror/lang-css"
import {html} from "@codemirror/lang-html"
import {javascript} from "@codemirror/lang-javascript"
import {json} from "@codemirror/lang-json"
import {markdown, markdownLanguage} from "@codemirror/lang-markdown"
import {python} from "@codemirror/lang-python"
import {sql} from "@codemirror/lang-sql"
import {xml} from "@codemirror/lang-xml"
import {languages} from "@codemirror/language-data"
import {autocompletion} from "@codemirror/autocomplete"
import {CmInstance} from "./CmInstance"

let CMInstances: { [id: string]: CmInstance } = {}

export function initCodeMirror(
    dotnetHelper: any,
    id: string,
    initialText: string,
    placeholderText: string,
    tabulationSize: number
) {
    var languageCompartment = new Compartment
    var tabSizeCompartment = new Compartment
    var indentUnitCompartment = new Compartment
    var placeholderCompartment = new Compartment

    var state = EditorState.create({
        doc: initialText,
        extensions: [
            basicSetup,
            languageCompartment.of(markdown({ base: markdownLanguage, codeLanguages: languages })),
            tabSizeCompartment.of(EditorState.tabSize.of(tabulationSize)),
            indentUnitCompartment.of(indentUnit.of(" ".repeat(tabulationSize))),
            keymap.of([indentWithTab]),
            EditorView.updateListener.of(async (update) => {
                if (update.docChanged) {
                    await dotnetHelper.invokeMethodAsync("DocChangedFromJS", update.state.doc.toString());
                }
                if (update.focusChanged) {
                    await dotnetHelper.invokeMethodAsync("FocusChangedFromJS", update.view.hasFocus);
                    if (!update.view.hasFocus)
                        await dotnetHelper.invokeMethodAsync("DocChangedFromJS", update.state.doc.toString());
                }
                if (update.selectionSet) {
                    await dotnetHelper.invokeMethodAsync("SelectionSetFromJS", update.state.selection.ranges.map(r => {return {from: r.from, to: r.to}}));
                }
            }),
            placeholderCompartment.of(placeholder(placeholderText)),
            autocompletion(),
        ]
    })

    var view = new EditorView({
        state,
        parent: document.getElementById(id),
    })

    CMInstances[id] = new CmInstance();

    CMInstances[id].dotNetHelper = dotnetHelper
    CMInstances[id].state = state
    CMInstances[id].view = view
    CMInstances[id].tabSizeCompartment = tabSizeCompartment
    CMInstances[id].indentUnitCompartment = indentUnitCompartment
    CMInstances[id].tabSize = tabulationSize
    CMInstances[id].language = languageCompartment
    CMInstances[id].placeholderCompartment = placeholderCompartment
}

export function setTabSize(id: string, size: number)
{
    CMInstances[id].tabSize = size;
    CMInstances[id].view.dispatch({
        effects: CMInstances[id].tabSizeCompartment.reconfigure(EditorState.tabSize.of(size))
    })
}

export function setIndentUnit(id: string, indentUnitString: string) {
    CMInstances[id].view.dispatch({
        effects: CMInstances[id].indentUnitCompartment.reconfigure(indentUnit.of(indentUnitString))
    })
}

export function setDoc(id: string, text: string)
{
    const transaction = CMInstances[id].view.state.update({
        changes: {from: 0, to: CMInstances[id].view.state.doc.length, insert: text}
    })
    CMInstances[id].view.dispatch(transaction)
}

export function setPlaceholderText(id: string, text: string) {
    CMInstances[id].view.dispatch({
        effects: CMInstances[id].placeholderCompartment.reconfigure(placeholder(text))
    })
}

export function dispose(id: string)
{
    CMInstances[id] = undefined;
    delete CMInstances[id];
}
