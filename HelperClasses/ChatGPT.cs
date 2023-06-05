using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenAI_API;
using static System.Net.Mime.MediaTypeNames;

namespace TestApp.HelperClasses
{
    public class ChatGPT
    {
        const string ChatGPTAPIKey = "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";
        public async Task<string?> CompleteChatAsync(string input)
        {
            var api = new OpenAI_API.OpenAIAPI(ChatGPTAPIKey);
            var result = await api.Chat.CreateChatCompletionAsync(input);
            string? strResult = result.Choices[0].Message.Content;
            return strResult;
        }
        public string ConvertStringToListOfQuestions( string input)
        {
            string regex1 = @"^\d+\.";
            string regex2 = @"^\d+\)";
            string regex;
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            List<string> strings  = new ();
            regex = regex1;
            MatchCollection matches = Regex.Matches(input, regex1, options);
            if(matches.Count <= 2)
            {
                matches = Regex.Matches(input, regex2, options);
                regex = regex2;
            }
            for(int i  = 0; i < matches.Count; i++)
            {
                string newString;
                if(i==0)
                {
                    newString = input.Substring(0, matches[i + 1].Index - matches[i + 1].Length);
                }
                else if(i==matches.Count - 1)
                {
                    newString = input.Substring(matches[i].Index);
                }
                else
                {
                    newString = input.Substring(matches[i].Index, matches[i + 1].Index - matches[i].Index);
                }
                strings.Add(newString);
            }
            List<Question> questions = new List<Question>();
            foreach (string question in strings)
            {
                var _question =  GetQuestion(question);
                questions.Add(_question);
            }
            foreach(Match match in matches)
            {
                Console.WriteLine("'{0}// found at index {1}.", match.Value, match.Index);
            }
            
            return "";
        }
        private Question GetQuestion(string input)
        {
            Question question = new Question();
            string regex1 = @"^\[a-z]\.";
            string regex2 = @"^\[a-z]\)$";
            List<string> answers = new List<string>();
            using (StringReader reader = new StringReader(input))
            {
                string line;
                bool firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if(firstLine)
                    {
                        question.QuestionText = line;
                        firstLine = false;
                    }
                    else if(line.StartsWith("Answer:") || line.StartsWith("Answers:") || line.StartsWith("A:"))
                    {
                        question.CorrectAnswer = line;
                    }
                    else
                    {
                        var match = Regex.Match(line, regex1);
                        string _regrex = regex1;
                        if(!match.Success)
                        {
                            match = Regex.Match(line, regex2);
                            _regrex = regex2;
                        }
                        if(match.Success)
                        {
                           question.Answers = line.Split(_regrex, StringSplitOptions.RemoveEmptyEntries);
                        }
                        else
                        {
                            answers.Add(line);
                        }
                    }
                    
                }
                if(answers.Count ==1 && question.CorrectAnswer==null)
                {
                    question.CorrectAnswer = answers[0];
                }
                else if (answers.Count > 0)
                {
                    question.Answers = answers.ToArray();
                }
            }
            return question;

        }

    }
    struct Question
    {
        public string QuestionText { get; set; }
        public string[] Answers { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
