using FluentResults;

namespace ResultsPatternMinimalApis.FluentModels;

public class RecordAlreadyExists(string message) : Error(message);

public class RecordCreated(string message) : Success(message);

public class RecordUpdated(string message) : Success(message);

public class RecordNotFound(string message) : Error(message);