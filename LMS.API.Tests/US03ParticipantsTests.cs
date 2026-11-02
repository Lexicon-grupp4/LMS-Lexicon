using System.Net;

namespace LMS.API.Tests;

[Collection("Api")]
public class US03ParticipantsTests(ApiFactory factory) : ApiTestBase(factory)
{
    [Fact]
    public async Task Student_sees_own_course_participants_only()
    {
        var student = await LoginAsync(Student);

        var response = await student.GetAsync("/api/courses/my-course/participants");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Erik", body);
        Assert.DoesNotContain("Lina", body);
    }

    [Fact]
    public async Task Teacher_sees_participants_of_a_course()
    {
        var teacher = await LoginAsync(Teacher);
        var courseId = await GetMyCourseIdAsync(await LoginAsync(Student));

        var response = await teacher.GetAsync($"/api/courses/{courseId}/participants");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Erik", body);
        Assert.DoesNotContain("Lina", body);
    }

    [Fact]
    public async Task Student_calls_course_participants_returns_403()
    {
        var student = await LoginAsync(Student);
        var courseId = await GetMyCourseIdAsync(student);

        var response = await student.GetAsync($"/api/courses/{courseId}/participants");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Participants_of_unknown_course_returns_404()
    {
        var teacher = await LoginAsync(Teacher);

        var response = await teacher.GetAsync("/api/courses/999999/participants");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Teacher_calls_my_course_returns_403()
    {
        var teacher = await LoginAsync(Teacher);

        var response = await teacher.GetAsync("/api/courses/my-course");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Teacher_calls_participants_returns_403()
    {
        var teacher = await LoginAsync(Teacher);

        var response = await teacher.GetAsync("/api/courses/my-course/participants");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task My_course_without_token_returns_401()
    {
        var response = await Anonymous().GetAsync("/api/courses/my-course");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Participants_without_token_returns_401()
    {
        var response = await Anonymous().GetAsync("/api/courses/my-course/participants");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
