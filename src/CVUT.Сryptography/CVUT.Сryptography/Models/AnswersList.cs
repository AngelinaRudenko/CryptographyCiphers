namespace CVUT.Сryptography.Models;

public class AnswersList
{
    private readonly CipherAnswer[] _answers;
    private int _currentCapacity = 0;

    public AnswersList(int capacity)
    {
        _answers = new CipherAnswer[capacity];
    }

    // TODO: make fully immutable
    public CipherAnswer[] Answers => _answers.ToArray();

    public void AddIfHigherProbability(CipherAnswer answer)
    {
        if (_currentCapacity < _answers.Length)
        {
            _answers[_currentCapacity] = answer;
            _currentCapacity++;
            return;
        }

        var leastPossibleAnswer = _answers.MinBy(x => x?.ProbabilityLogSum);

        if (answer.ProbabilityLogSum > leastPossibleAnswer?.ProbabilityLogSum)
        {
            var minIndex = Array.IndexOf(_answers, leastPossibleAnswer);
            _answers[minIndex] = answer;
        }
    }
}