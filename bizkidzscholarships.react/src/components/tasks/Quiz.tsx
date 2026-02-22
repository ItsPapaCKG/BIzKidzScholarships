import { useEffect, useState } from "react";
import type { TaskQuestion, TaskQuizAnswers, UserAnswer } from "../../models/ViewModels";
import { GetQuizQuestions, SubmitQuizToServer } from "../../services/UserDataService";
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

        func();
    }, []);

    const saveAnswer = (index: number, input: string[]) => {
        let a: UserAnswer = {
            questionId: questions[index].questionId,
            answer: input
        };

        setAnswers(prev =>
            prev.map((item, idx) =>
                idx == index ? a : item
            )
        );
        setInput([]);
    }

    const toggleCheckbox = (value: string) => {
        let answerArray = currAnswer.answer;

        if (answerArray.includes(value)) {
            let i = answerArray.indexOf(value);

            answerArray.splice(i, 1);

            let uA: UserAnswer = {
                questionId: currQuestion.questionId,
                answer: answerArray
            }

            setAnswers(prev =>
                prev.map((item, idx) =>
                    idx == currentQuestion ? uA : item
                )
            );

            return;
        }

        answerArray.push(value);

        let uA: UserAnswer = {
            questionId: currQuestion.questionId,
            answer: answerArray
        }

        setAnswers(prev =>
            prev.map((item, idx) =>
                idx == currentQuestion ? uA : item
            )
        );

        return;
    }

    const incrementQuestion = () => {
        if (currentQuestion < questions.length) {
            setCurrentQuestion(currentQuestion + 1);
            return;
        }

        let submission: TaskQuizAnswers = {
            taskId: task!.TaskId,
            answers: answers
        }

        let submit = async () => {
            let res = await SubmitQuizToServer(submission);
        }

        submit();
    }

    const previousQuestion = () => {
        if (currentQuestion > -1) {
            setCurrentQuestion(currentQuestion - 1);
            return;
        }
    }

    let currQuestion = questions[currentQuestion];
    let currAnswer = answers[currentQuestion];

    let minimumInputsSelected = currAnswer.answer.length > 0;
    let lastQuestion = currentQuestion == questions.length - 1;

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
                            <img src={currQuestion.promptImageKey} className="card-img-top w-100 h-100" style={{ objectFit: "cover", objectPosition: "0% 0%" }}></img>

                            <div className="card-body">
                                <h1>Question { currentQuestion + 1 }</h1>
                                <p>{ currQuestion.prompt }</p>

                                {/* Radio */}
                                {!currQuestion.multi && Object.entries(currQuestion.options!).map(([key, prompt]) => (
                                    <label key={`question-${key}`} style={{ display: "block" }}>
                                        <input
                                            type="radio"
                                            name="my-radio-group"
                                            value={key}
                                            checked={currAnswer.answer[0] === key}
                                            onChange={() => saveAnswer(currentQuestion, [key])}
                                        />
                                        {prompt}
                                    </label>
                                ))}

                                {currQuestion.multi && Object.entries(currQuestion.options!).map(([key, prompt]) => (
                                    <label key={`question-multi-${key}`} style={{ display: "block" }}>
                                        <input
                                            type="checkbox"
                                            value={key}
                                            checked={currAnswer.answer.includes(key)}
                                            onChange={() => toggleCheckbox(key)}
                                        />
                                        {prompt}
                                    </label>
                                )) }

                            </div>
                            <div className="card-body">
                                <div className="row">
                                    <div className="col">
                                        { currentQuestion != 1 && <button type="button" className="btn btn-light" onClick={previousQuestion}>Previous Question</button>}
                                    </div>

                                    <div className="col">
                                        <button type="button" className="btn btn-success" onClick={incrementQuestion} disabled={!minimumInputsSelected}>{lastQuestion ? "Submit Quiz" : "Next Question"}</button>
                                    </div>
                                </div>
                            </div>
                        </>)}
                </div>)}
        </>
    );
}

export default Quiz;

