Job interview repo documentation:

app launches with scalar ui for local testing

Project is organized into simple monolith based around vertical slices for features

Mediatr community free version is added for familiarity and convenience.
For something serious we should probably implement our own or use completely different approach

Carter library is added for convenience purpose with minimal apis and vertical slices.

Result pattern is added and it's just something simple i copy around my personal projects when I want a bit cleaner responses.

Mapster is added for mapping speed and simplicity here, but usually would prefer manual mapping

Db seeder is kept basic, would usually come from json files, mapped to dtos and validated before inserts.

Tests are not included, but if needed, integrations tests would be added.
If unit tests for some reason are requirement I would use repository pattern.

All wallet transaction would be probably coming from some service, in this example it's just a mock.
