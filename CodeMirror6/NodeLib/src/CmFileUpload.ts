import { EditorView } from "@codemirror/view"
import { EditorSelection } from '@codemirror/state'
import { CmSetup } from "./CmSetup"
import { CMInstances } from "./CmInstance"


export function getFileUploadExtensions(id: string, setup: CmSetup)
{
    const overlayId = `${id}-file-upload`
    // check if the document already contains the overlay
    if (document.getElementById(overlayId)) return []

    const overlay = document.createElement("div")
    overlay.id = overlayId
    overlay.innerHTML = `<i class="${setup.fileIcon} me-2"></i> Drop files here`
    overlay.style.cssText = `
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        color: white;
        display: none;
        align-items: center;
        justify-content: center;
        z-index: 10;
        pointer-events: none;
        font-size: x-large;
    `
    let depth = 0

    // Append the overlay to your CodeMirror container
    const editorContainer = document.getElementById(id) // Replace with your actual container ID
    editorContainer.style.position = 'relative'
    editorContainer.appendChild(overlay)

    const dragAndDropHandler = EditorView.domEventHandlers({
        dragenter(event, view) {
            event.preventDefault()
            overlay.style.display = 'flex'
            depth++
        },
        dragleave(event, view) {
            event.preventDefault();
            depth--
            if (depth === 0) {
                overlay.style.display = 'none'
            }
        },
        dragover(event, view) {
            event.preventDefault()
            overlay.style.display = 'flex'
        },
        drop(event, view) {
            const transfer = event.dataTransfer
            if (transfer?.files) {
                overlay.style.display = 'none'
                event.preventDefault()
                event.stopPropagation()
                const { clientX, clientY } = event
                const pos = view.posAtCoords({ x: clientX, y: clientY })
                if (pos !== null) {
                    let newSelection = EditorSelection.single(pos)
                    view.dispatch({
                        selection: newSelection
                    });
                }
                uploadFiles(transfer.files, view)
                depth = 0
            }
        }
    })

    const pasteHandler = EditorView.domEventHandlers({
        paste(event, view) {
            const transfer = event.clipboardData
            if (transfer?.files && transfer.files.length > 0) {
                event.preventDefault()
                uploadFiles(transfer.files, view)
            }
        }
    })

    async function uploadFiles(files: FileList, view: EditorView) {
        const fileUrls = []
        for (let i = 0; i < files.length; i++) {
            const file = files[i]
            const fileUrl = await uploadFileWithDotnet(id, file)
            fileUrls.push(fileUrl)
            console.log("Uploaded file:", fileUrl)
            const fileName = files[0].name
            var imageChar = file.type.indexOf("image/") === 0 ? "!" : ""
            var imageLink = `\n${imageChar}[${fileName}](${fileUrl})\n`
            const ranges = view.state.selection.ranges;
            const lastRange = ranges[ranges.length - 1];

            view.dispatch(
                {
                    changes: { from: view.state.selection.main.from, insert: imageLink },
                    selection: { anchor: lastRange.from + imageLink.length, head: lastRange.from + imageLink.length }
                },
                {
                    scrollIntoView: true
                }
            )
        }
        return fileUrls
    }

    async function uploadFileWithDotnet(id: string, file: File): Promise<string> {
        const arrayBuffer = await file.arrayBuffer()
        const byteArray = new Uint8Array(arrayBuffer)
        const lastModifiedDate = new Date(file.lastModified)
        return await CMInstances[id].dotNetHelper.invokeMethodAsync('UploadFileFromJS', byteArray, file.name, file.type, lastModifiedDate)
    }

    return [
        dragAndDropHandler, pasteHandler, //selectionField
    ]
}