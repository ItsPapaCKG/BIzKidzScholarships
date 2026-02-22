import { useEffect, useState } from "react";
import type { TaskQuestion, UserAnswer } from "../../models/ViewModels";
import { GetQuizQuestions } from "../../services/UserDataService";
import { UseTaskContext } from "../../contexts/TaskViewContext";

type Question = TaskQuestion[]

function Quiz() {

    const context = UseTaskContext();

    const [questions, setQuestions] = useState<TaskQuestion[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [currentQuestion, setCurrentQuestion] = useState<number>(0);
    const [task] = [context.viewedTask];
    const [answers, setAnswers] = useState<UserAnswer[]>([]);
    const [input, setInput] = useState<string[]>([]);
    /*const [selectedRadio, setSelectedRadio] = useState<string | null>()*/

    useEffect(() => {
        let func = async () => {

            if (task == null) {
                setError("Error: no task defined");
                return;
            }

            let res = await GetQuizQuestions(task.TaskId);

            if (res.success) {
                setQuestions(res.data!);
                return;
            }

            setError(res.error.message!);
        }
    }, []);

    const saveAnswer = (index: number, input: string[]) => {
        let answer: UserAnswer = {
            questionId: questions[index].questionId,
            answer: input
        };

        setAnswers(prev =>
            prev.map((item, idx) =>
                idx == index ? answer : item
            )
        );
        setInput([]);
    }

    const toggleCheckbox = (index: number, value: string) => { 


    }

    let question = questions[currentQuestion];
    let answer = answers[currentQuestion];

    return (
        <>
            {error ?
                (<>
                    <p className="text-danger fs-8">{ error }</p>
                </>)

                :

                (<div className="card">
                    {currentQuestion == -1 ?
                        (<div className="card-body">
                            <h1>{task!.TaskPromptTitle}</h1>
                            <p>{task!.TaskPromptSubtitle}</p>
                        </div>)
                            :
                        (<>
                            <img src={question.promptImageKey} className="card-img-top w-100 h-100" style={{ objectFit: "cover", objectPosition: "0% 0%" }}></img>

                            <div className="card-body">
                                <h1>Question { currentQuestion + 1 }</h1>
                                <p>{ question.prompt }</p>

                                {/* Radio */}
                                {!question.multi && Object.entries(question.options!).map(([key, prompt]) => (
                                    <label key={`question-${key}`} style={{ display: "block" }}>
                                        <input
                                            type="radio"
                                            name="my-radio-group"
                                            value={key}
                                            checked={answer.answer[0] === key}
                                            onChange={() => saveAnswer(currentQuestion, [key])}
                                        />
                                        {prompt}
                                    </label>
                                ))}

                                {question.multi && Object.entries(question.options!).map(([key, prompt]) => (
                                    <label key={`question-multi-${key}`} style={{ display: "block" }}>
                                        <input
                                            type="checkbox"
                                            value={key}
                                            checked={answer.answer.includes(key)}
                                            onChange={() => toggleCheckbox(option.id)}
                                        />
                                        {option.label}
                                    </label>
                                )) }

                            </div>
                        </>)}
                </div>)}
        </>
    );
}

export default Quiz;

