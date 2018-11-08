# Canvas connector for .NET

This is a simple wrapper for parts of the Canvas API, written in C#. Example usage:

```csharp
var conn = new CanvasConnector(canvasUrl, token);
var course = conn.FindCourseById(25);
            
// find and edit assignment
var assign = course.Assignments.First(a => a.Name == "Homework");
assign.PointsPossible = 25;
assign.Save();

// create a course
var newCourse = new Course(conn)
{
    Name = "Testcourse",
    CourseCode = "Test",
    AccountID = 3
};
newCourse.Save();
```