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
public partial class ChatHeader : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h2", "1.1", new Dictionary<string, string>(), "Chat Room"), (new MObject(unreadCount>0)) ? new VElement("span", "1.2.1", new Dictionary<string, string> { ["id"] = "unread-badge", ["class"] = "unread-badge" }, new VNode[]
        {
            new VText($"{(unreadCount)}", "1.2.1.1")
        }) : new VNull("1.2"));
    }
}

[Component]
public partial class MessageList : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("h3", "1.1", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Messages (", "1.1.1"),
            new VText($"{(messages.length)}", "1.1.2"),
            new VText(")", "1.1.3")
        }), new VElement("div", "1.2", new Dictionary<string, string> { ["id"] = "messages-container" }, new VNode[]
        {
            new VText($"{(messages.length===0?(<pid="no-messages">No messages yet. Start the conversation!</p>):(messages.map(msg=>(<divkey={msg.id}className="message"data-message-id={msg.id}><strong>{msg.author}:</strong>{msg.text}<spanclassName="timestamp">{msg.timestamp}</span></div>))))}", "1.2.1")
        }));
    }
}

[Component]
public partial class MessageInput : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        return MinimactHelpers.createElement("div", null, new VElement("textarea", "1.1", new Dictionary<string, string> { ["id"] = "message-textarea", ["value"] = $"{(draft)}", ["oninput"] = "Handle0", ["onkeypress"] = "handleKeyPress", ["placeholder"] = "Type a message..." }), new VElement("button", "1.2", new Dictionary<string, string> { ["id"] = "send-btn", ["type"] = "button", ["onclick"] = "onSend", ["disabled"] = $"{(!draft.trim())}" }, "Send"), new VElement("p", "1.3", new Dictionary<string, string>(), new VNode[]
        {
            new VText("Characters:", "1.3.1"),
            new VElement("span", "1.3.2", new Dictionary<string, string> { ["id"] = "char-count" }, new VNode[]
            {
                new VText($"{(draft.length)}", "1.3.2.1")
            })
        }));
    }

    public void handleKeyPress()
    {
        if(e.key==='Enter'&&!e.shiftKey){e.preventDefault();onSend();}
    }

    public void Handle0()
    {
        SetState(nameof(state), 'draft',e.target.value);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleKeyPress"] = @"function () {\n  { if(e.key==='Enter'&&!e.shiftKey){e.preventDefault();onSend();} };\n}",
            ["Handle0"] = @"function () {\n  setState('draft',e.target.value);\n}"
        };
    }
}

[Component]
public partial class ChatPage : MinimactComponent
{
    protected override VNode Render()
    {
        StateManager.SyncMembersToState(this);

        var _MessageInput_draft_reader = State["MessageInput.draft"];
        var _MessageList_messages_reader = State["MessageList.messages"];
        var _ChatHeader_unreadCount_reader = State["ChatHeader.unreadCount"];

        return MinimactHelpers.createElement("div", null,         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.1",
            InitialState = new Dictionary<string, object> {              }
        },         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.2",
            InitialState = new Dictionary<string, object> {              }
        }, (new MObject(isTyping)) ? new VElement("div", "1.3.1", new Dictionary<string, string> { ["id"] = "typing-indicator", ["class"] = "typing-indicator" }, "You are typing...") : new VNull("1.3"),         new VComponentWrapper
                {
                        ComponentName = "Component",
                        ComponentType = "Component",
                        HexPath = "1.4",
            InitialState = new Dictionary<string, object> {              }
        }, new VElement("div", "1.5", new Dictionary<string, string> { ["class"] = "controls" }, new VNode[]
        {
            new VElement("button", "1.5.1", new Dictionary<string, string> { ["id"] = "add-bot-btn", ["type"] = "button", ["onclick"] = "handleAddBotMessage" }, "Add Bot Message"),
            new VElement("button", "1.5.2", new Dictionary<string, string> { ["id"] = "clear-btn", ["type"] = "button", ["onclick"] = "handleClear", ["disabled"] = $"{(messages.length===0&&draft==="")}" }, "Clear All")
        }), new VElement("div", "1.6", new Dictionary<string, string> { ["id"] = "status", ["class"] = "status" }, new VNode[]
        {
            new VElement("p", "1.6.1", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Message Count:", "1.6.1.1"),
                new VElement("span", "1.6.1.2", new Dictionary<string, string> { ["id"] = "message-count" }, new VNode[]
                {
                    new VText($"{(messages.length)}", "1.6.1.2.1")
                })
            }),
            new VElement("p", "1.6.2", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Draft Length:", "1.6.2.1"),
                new VElement("span", "1.6.2.2", new Dictionary<string, string> { ["id"] = "draft-length" }, new VNode[]
                {
                    new VText($"{(draft.length)}", "1.6.2.2.1")
                })
            }),
            new VElement("p", "1.6.3", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Is Typing:", "1.6.3.1"),
                new VElement("span", "1.6.3.2", new Dictionary<string, string> { ["id"] = "is-typing" }, new VNode[]
                {
                    new VText($"{(isTyping?'Yes':'No')}", "1.6.3.2.1")
                })
            }),
            new VElement("p", "1.6.4", new Dictionary<string, string>(), new VNode[]
            {
                new VText("Unread:", "1.6.4.1"),
                new VElement("span", "1.6.4.2", new Dictionary<string, string> { ["id"] = "unread-count" }, new VNode[]
                {
                    new VText($"{(State["ChatHeader.unreadCount"]||0)}", "1.6.4.2.1")
                })
            })
        }));
    }

    public void handleSend()
    {
        if(draft.trim()){constnewMessage={id:Date.now(),text:draft,author:"Me",timestamp:newDate().toLocaleTimeString()};SetState(nameof(state), "MessageList.messages",[...messages,newMessage]);SetState(nameof(state), "MessageInput.draft","");SetState(nameof(state), "ChatHeader.unreadCount",0);}
    }

    public void handleAddBotMessage()
    {
        constbotMessage={id:Date.now(),text:"This is a bot message",author:"Bot",timestamp:newDate().toLocaleTimeString()};SetState(nameof(state), "MessageList.messages",[...messages,botMessage]);SetState(nameof(state), "ChatHeader.unreadCount",(state["ChatHeader.unreadCount"]||0)+1);
    }

    public void handleClear()
    {
        SetState(nameof(state), "MessageList.messages",[]);SetState(nameof(state), "MessageInput.draft","");SetState(nameof(state), "ChatHeader.unreadCount",0);
    }

    /// <summary>
    /// Returns JavaScript event handlers for client-side execution
    /// These execute in the browser with bound hook context
    /// </summary>
    protected override Dictionary<string, string> GetClientHandlers()
    {
        return new Dictionary<string, string>
        {
            ["handleSend"] = @"function () {\n  { if(draft.trim()){constnewMessage={id:Date.now(),text:draft,author:""Me"",timestamp:newDate().toLocaleTimeString()};setState(""MessageList.messages"",[...messages,newMessage]);setState(""MessageInput.draft"","""");setState(""ChatHeader.unreadCount"",0);} };\n}",
            ["handleAddBotMessage"] = @"function () {\n  { constbotMessage={id:Date.now(),text:""This is a bot message"",author:""Bot"",timestamp:newDate().toLocaleTimeString()};setState(""MessageList.messages"",[...messages,botMessage]);setState(""ChatHeader.unreadCount"",(state[""ChatHeader.unreadCount""]||0)+1); };\n}",
            ["handleClear"] = @"function () {\n  { setState(""MessageList.messages"",[]);setState(""MessageInput.draft"","""");setState(""ChatHeader.unreadCount"",0); };\n}"
        };
    }
}
