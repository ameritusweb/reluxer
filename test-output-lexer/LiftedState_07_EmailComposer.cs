using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minimact.AspNetCore.Core;
using Minimact.AspNetCore.Rendering;
using Minimact.AspNetCore.Extensions;
using MinimactHelpers = Minimact.AspNetCore.Core.Minimact;

namespace MinimactTest.Components;

[Component]
public partial class RecipientList : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), "To:"), new VElement("div", "1.2", new Dictionary<string, string> { ["id"] = "recipients" }, new VNode[]
        {
                        MinimactHelpers.createFragment(recipients.Select((email, string) =>
                new VElement("div", "1.2.1.item", new Dictionary<string, string> { ["key"] = $"{(idx)}", ["class"] = "recipient-chip", ["data-recipient-index"] = $"{(idx)}" }, new VNode[]
                {
                    new VText($"{(email)}", "1.2.1.item.1"),
                    new VElement("button", "1.2.1.item.2", new Dictionary<string, string> { ["type"] = "button", ["class"] = "remove-recipient-btn", ["onclick"] = "Handle0" }, "×")
                })
            ).ToArray())
        }), new VElement("button", "1.3", new Dictionary<string, string> { ["id"] = "add-recipient-btn", ["type"] = "button", ["onclick"] = "handleAdd" }, "Add Recipient"));
    }

    public void handleAdd()
    {
        constemail=prompt('Enter recipient email:');if(email&&email.includes('@')){SetState(nameof(state), 'recipients',[...recipients,email]);}
    }

    public void handleRemove()
    {
        SetState(nameof(state), 'recipients',recipients.filter((_:string,i:number)=>i!==index));
    }

    public void Handle0()
    {
        handleRemove(idx);
    }

    public void Handle1()
    {
        handleRemove(idx);
    }

    public void Handle2()
    {
        handleFileRemove(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleAdd"] = @"function () {\n  { constemail=prompt('Enter recipient email:');if(email&&email.includes('@')){setState('recipients',[...recipients,email]);} };\n}",
            ["handleRemove"] = @"function () {\n  { setState('recipients',recipients.filter((_:string,i:number)=>i!==index)); };\n}",
            ["Handle0"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle1"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle2"] = @"function () {\n  handleFileRemove(idx);\n}"
        };
    }
}

[Component]
public partial class SubjectLine : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("label", "1.1", new Dictionary<string, string> { ["for"] = "subject-input" }, "Subject:"), new VElement("input", "1.2", new Dictionary<string, string> { ["id"] = "subject-input", ["type"] = "text", ["value"] = $"{(text)}", ["oninput"] = "Handle0", ["placeholder"] = "Enter subject..." }));
    }

    public void Handle0()
    {
        SetState(nameof(state), 'text',e.target.value);
    }

    public void Handle1()
    {
        handleRemove(idx);
    }

    public void Handle2()
    {
        handleFileRemove(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setState('text',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle2"] = @"function () {\n  handleFileRemove(idx);\n}"
        };
    }
}

[Component]
public partial class MessageBody : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("label", "1.1", new Dictionary<string, string> { ["for"] = "body-textarea" }, "Message:"), new VElement("textarea", "1.2", new Dictionary<string, string> { ["id"] = "body-textarea", ["value"] = $"{(content)}", ["oninput"] = "Handle0", ["placeholder"] = "Enter message...", ["rows"] = $"{(10)}" }), new VElement("p", "1.3", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Characters:", "1.3.1"),
            new VElement("span", "1.3.2", new Dictionary<string, string> { ["id"] = "body-char-count" }, new VNode[]
            {
                new VText($"{(content.length)}", "1.3.2.1")
            })
        }));
    }

    public void Handle0()
    {
        SetState(nameof(state), 'content',e.target.value);
    }

    public void Handle1()
    {
        handleRemove(idx);
    }

    public void Handle2()
    {
        handleFileRemove(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["Handle0"] = @"function () {\n  setState('content',e.target.value);\n}",
            ["Handle1"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle2"] = @"function () {\n  handleFileRemove(idx);\n}"
        };
    }
}

[Component]
public partial class AttachmentPanel : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Attachments (", "1.1.1"),
            new VElement("span", "1.1.2", new Dictionary<string, string> { ["id"] = "attachment-size" }, new VNode[]
            {
                new VText($"{(formatBytes(totalSize))}", "1.1.2.1")
            }),
            new VText("/", "1.1.3"),
            new VText($"{(formatBytes(maxSize))}", "1.1.4"),
            new VText(")", "1.1.5")
        }), new VElement("button", "1.2", new Dictionary<string, string> { ["id"] = "add-file-btn", ["type"] = "button", ["onclick"] = "handleFileAdd" }, "Add File (Simulated)"), new VElement("ul", "1.3", new Dictionary<string, string> { ["id"] = "file-list" }, new VNode[]
        {
                        MinimactHelpers.createFragment(files.Select((file, any) =>
                new VElement("li", "1.3.1.item", new Dictionary<string, string> { ["key"] = $"{(idx)}", ["data-file-index"] = $"{(idx)}" }, new VNode[]
                {
                    new VText($"{(file.name)}", "1.3.1.item.1"),
                    new VText("(", "1.3.1.item.2"),
                    new VText($"{(formatBytes(file.size))}", "1.3.1.item.3"),
                    new VText(")", "1.3.1.item.4"),
                    new VElement("button", "1.3.1.item.5", new Dictionary<string, string> { ["type"] = "button", ["class"] = "remove-file-btn", ["onclick"] = "Handle0" }, "Remove")
                })
            ).ToArray())
        }));
    }

    public void handleFileAdd()
    {
        constfileSize=Math.floor(Math.random()*5000000)+1000000;constfileName=`file_${Date.now()}.pdf`;constnewFiles=[...files,{name:fileName,size:fileSize}];constnewSize=newFiles.reduce((sum:number,f:any)=>sum+f.size,0);if(newSize<=maxSize){SetState(nameof(state), 'files',newFiles);SetState(nameof(state), 'totalSize',newSize);}else{alert(`File too large! Would exceed ${formatBytes(maxSize)} limit.`);}
    }

    public void handleFileRemove()
    {
        constnewFiles=files.filter((_:any,i:number)=>i!==index);constnewSize=newFiles.reduce((sum:number,f:any)=>sum+f.size,0);SetState(nameof(state), 'files',newFiles);SetState(nameof(state), 'totalSize',newSize);
    }

    public void Handle0()
    {
        handleFileRemove(idx);
    }

    public void Handle1()
    {
        handleRemove(idx);
    }

    public void Handle2()
    {
        handleFileRemove(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleFileAdd"] = @"function () {\n  { constfileSize=Math.floor(Math.random()*5000000)+1000000;constfileName=`file_${Date.now()}.pdf`;constnewFiles=[...files,{name:fileName,size:fileSize}];constnewSize=newFiles.reduce((sum:number,f:any)=>sum+f.size,0);if(newSize<=maxSize){setState('files',newFiles);setState('totalSize',newSize);}else{alert(`File too large! Would exceed ${formatBytes(maxSize)} limit.`);} };\n}",
            ["handleFileRemove"] = @"function () {\n  { constnewFiles=files.filter((_:any,i:number)=>i!==index);constnewSize=newFiles.reduce((sum:number,f:any)=>sum+f.size,0);setState('files',newFiles);setState('totalSize',newSize); };\n}",
            ["Handle0"] = @"function () {\n  handleFileRemove(idx);\n}",
            ["Handle1"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle2"] = @"function () {\n  handleFileRemove(idx);\n}"
        };
    }
}

[Component]
public partial class EmailComposer : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _RecipientList_recipients_reader = State["RecipientList.recipients"];
        var _SubjectLine_text_reader = State["SubjectLine.text"];
        var _MessageBody_content_reader = State["MessageBody.content"];
        var _AttachmentPanel_files_reader = State["AttachmentPanel.files"];
        var _AttachmentPanel_totalSize_reader = State["AttachmentPanel.totalSize"];

        return MinimactHelpers.createElement("div", null, new VElement("h1", "1.1", new Dictionary<string, string>(), "New Message"), (errors.length>0) ? new VElement("div", "1.2.1", new Dictionary<string, string> { ["id"] = "error-panel", ["class"] = "error-panel" }, new VNode[]
        {
            new VElement("strong", "1.2.1.1", new Dictionary<string, string>(), "Cannot send:"),
            new VElement("ul", "1.2.1.2", new Dictionary<string, string>(), new VNode[]
            {
                                MinimactHelpers.createFragment(errors.Select((error, idx) =>
                    new VElement("li", "1.2.1.2.1.item", new Dictionary<string, string> { ["key"] = $"{(idx)}" }, new VNode[]
                    {
                        new VText($"{(error)}", "1.2.1.2.1.item.1")
                    })
                ).ToArray())
            })
        }) : new VNull("1.2"), (totalSize>MAX_ATTACHMENT_SIZE*0.8) ? new VElement("div", "1.3.1", new Dictionary<string, string> { ["id"] = "warning-panel", ["class"] = "warning-panel" }) : new VNull("1.3"),         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.5",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.6",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.7",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.8", new Dictionary<string, string> { ["class"] = "send-controls" }, new VNode[]
        {
            new VElement("button", "1.8.1", new Dictionary<string, string> { ["id"] = "send-btn", ["class"] = "btn-send", ["type"] = "button", ["onclick"] = "handleSend", ["disabled"] = $"{(!canSend)}" }, "Send Email"),
            new VElement("button", "1.8.2", new Dictionary<string, string> { ["id"] = "reset-btn", ["type"] = "button", ["onclick"] = "handleReset" }, "Reset All"),
            new VElement("span", "1.8.3", new Dictionary<string, string> { ["id"] = "recipient-count", ["class"] = "recipient-count" }, new VNode[]
            {
                new VText("To:", "1.8.3.1"),
                new VText($"{(recipients.length)}", "1.8.3.2"),
                new VText("recipient", "1.8.3.3"),
                new VText($"{(recipients.length!==1?'s':'')}", "1.8.3.4")
            })
        }), new VElement("div", "1.9", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.9.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Recipients:", "1.9.1.1"),
                new VElement("span", "1.9.1.2", new Dictionary<string, string> { ["id"] = "status-recipients" }, new VNode[]
                {
                    new VText($"{(recipients.length)}", "1.9.1.2.1")
                })
            }),
            new VElement("p", "1.9.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Subject Length:", "1.9.2.1"),
                new VElement("span", "1.9.2.2", new Dictionary<string, string> { ["id"] = "status-subject" }, new VNode[]
                {
                    new VText($"{(subject.length)}", "1.9.2.2.1")
                })
            }),
            new VElement("p", "1.9.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Body Length:", "1.9.3.1"),
                new VElement("span", "1.9.3.2", new Dictionary<string, string> { ["id"] = "status-body" }, new VNode[]
                {
                    new VText($"{(body.length)}", "1.9.3.2.1")
                })
            }),
            new VElement("p", "1.9.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Attachments:", "1.9.4.1"),
                new VElement("span", "1.9.4.2", new Dictionary<string, string> { ["id"] = "status-attachments" }, new VNode[]
                {
                    new VText($"{(attachments.length)}", "1.9.4.2.1")
                })
            }),
            new VElement("p", "1.9.5", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Total Size:", "1.9.5.1"),
                new VElement("span", "1.9.5.2", new Dictionary<string, string> { ["id"] = "status-size" }, new VNode[]
                {
                    new VText($"{(formatBytes(totalSize))}", "1.9.5.2.1")
                })
            }),
            new VElement("p", "1.9.6", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Can Send:", "1.9.6.1"),
                new VElement("span", "1.9.6.2", new Dictionary<string, string> { ["id"] = "status-can-send" }, new VNode[]
                {
                    new VText($"{(canSend?'Yes':'No')}", "1.9.6.2.1")
                })
            })
        }));
    }

    public void handleSend()
    {
        if(canSend){constemail={recipients,subject,body,attachments};console.log('Sending email:',email);alert('Email sent successfully!');SetState(nameof(state), "RecipientList.recipients",[]);SetState(nameof(state), "SubjectLine.text","");SetState(nameof(state), "MessageBody.content","");SetState(nameof(state), "AttachmentPanel.files",[]);SetState(nameof(state), "AttachmentPanel.totalSize",0);}
    }

    public void handleClearAttachments()
    {
        SetState(nameof(state), "AttachmentPanel.files",[]);SetState(nameof(state), "AttachmentPanel.totalSize",0);
    }

    public void handleReset()
    {
        SetState(nameof(state), "RecipientList.recipients",[]);SetState(nameof(state), "SubjectLine.text","");SetState(nameof(state), "MessageBody.content","");SetState(nameof(state), "AttachmentPanel.files",[]);SetState(nameof(state), "AttachmentPanel.totalSize",0);
    }

    public void Handle0()
    {
        handleRemove(idx);
    }

    public void Handle1()
    {
        handleFileRemove(idx);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleSend"] = @"function () {\n  { if(canSend){constemail={recipients,subject,body,attachments};console.log('Sending email:',email);alert('Email sent successfully!');setState(""RecipientList.recipients"",[]);setState(""SubjectLine.text"","""");setState(""MessageBody.content"","""");setState(""AttachmentPanel.files"",[]);setState(""AttachmentPanel.totalSize"",0);} };\n}",
            ["handleClearAttachments"] = @"function () {\n  { setState(""AttachmentPanel.files"",[]);setState(""AttachmentPanel.totalSize"",0); };\n}",
            ["handleReset"] = @"function () {\n  { setState(""RecipientList.recipients"",[]);setState(""SubjectLine.text"","""");setState(""MessageBody.content"","""");setState(""AttachmentPanel.files"",[]);setState(""AttachmentPanel.totalSize"",0); };\n}",
            ["Handle0"] = @"function () {\n  handleRemove(idx);\n}",
            ["Handle1"] = @"function () {\n  handleFileRemove(idx);\n}"
        };
    }
}
