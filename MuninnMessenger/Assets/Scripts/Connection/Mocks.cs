using System;
using System.Collections.Generic;

public class Mocks
{
    public static readonly string[] SAMPLE_MESSAGES = new string[]
    {
        "Hello, guys!",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        "I built a registration form for a mobile game using Unity 5.1. To do that, I use Unity UI components: ScrollRect + Autolayout (Vertical layout)",
        "The Venus de Milo or Aphrodite of Melos is an ancient Greek marble sculpture that was created during the Hellenistic period. Its exact dating is uncertain, but the modern consensus places it in the 2nd century BC, perhaps between 160 and 110 BC. It was rediscovered in 1820 on the island of Milos, Greece, and has been displayed at the Louvre Museum since 1821. Since the statue's discovery, it has become one of the most famous works of ancient Greek sculpture in the world.",
        "So sad https://en.wikipedia.org/wiki/Michelle_Trachtenberg",
    };

    public static readonly User[] USERS = new User[]
    {
        new User() {
            Id = 1,
            Name = "John",
            AvatarUrl = "https://simp6.jpg5.su/images3/535486ff71b1b4dda4530b.jpg",
        },
        new User() {
            Id = 2,
            Name = "Bob",
            AvatarUrl = "https://simp6.jpg5.su/images3/3840x2881_9a8d71d08b04774eaba2ec0750d2abf7a2b5782fdbfb97f5.jpg",
        },
        new User() {
            Id = 3,
            Name = "Tommy",
            AvatarUrl = "https://simp6.jpg5.su/images3/DSC07742d056b93b61ecfda8.md.jpg",
        },
        new User() {
            Id = 4,
            Name = "Alice",
            AvatarUrl = "https://fapello.com/content/y/o/yourcorarossi/1000/yourcorarossi_0450.jpg",
        }
    };

    public static readonly List<Chat> CHATS = new List<Chat>()
    {
        new Chat()
        {
            Id = 0,
            Title = "Cool chat",
            AvatarUrl = "https://img10.joyreactor.cc/pics/post/Moonsimorfin-Photo-Porn-%D0%9F%D0%BE%D1%80%D0%BD%D0%BE-8809975.png",
            Participants = new List<User> { USERS[0], USERS[1], USERS[2] },
            Messages = new List<Message>(),
            CreatedAt = new DateTime(2024, 4, 23),
        }
    };

    public static Chat GetNewChat()
    {
        var chat = new Chat()
        {
            Id = CHATS[^1].Id + 1,
            Title = $"Cool chat #{UnityEngine.Random.Range(1, 1000)}",
            AvatarUrl = $"https://fapello.com/content/y/o/yourcorarossi/1000/yourcorarossi_{UnityEngine.Random.Range(1, 440):0000}.jpg",
            Participants = new List<User> { USERS[0] },
            Messages = new List<Message>(),
            CreatedAt = DateTime.Now,
        };

        CHATS.Add(chat);

        return chat;
    }

    public static Message GetNewMessage()
    {
        var attachments = new List<Attachment>();

        for(int i = 0; i < UnityEngine.Random.Range(0, 4); i++)
        {
            attachments.Add(new Attachment()
            {
                Url = $"https://fapello.com/content/n/e/neyrodesu/3000/neyrodesu_{UnityEngine.Random.Range(2001, 2700)}.jpg",
                FileType = Attachment.Type.Image
            });
        }

        var chat = CHATS.GetRandom();
        var message = new Message(chat.Id)
        {
            ReplyTo = UnityEngine.Random.Range(0, 2) == 0 ? null : (long)UnityEngine.Random.Range(0, Message.MAX_ID),
            Author = USERS.GetRandom(),
            Text = SAMPLE_MESSAGES.GetRandom(),
            Sent = DateTime.Now,
            Attachments = attachments
        };

        chat.Messages.Add(message);

        return message;
    }
}