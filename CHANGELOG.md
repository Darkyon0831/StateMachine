#   Changelog

# [1.0.3] - 2023-05-06

### Added

- Visual inducation of what state is currently running

### Changed

- Changed the StateCondition interface to have a Begin and End condition
- Redefined the DefaultStateChoser to two different components, OnlyBeginChoser and BeginAndEndChoser 

# [1.0.2] - 2023-05-05

### Added

- optional PreRun override to state executer
- optional PostRun override to state executer

### Changed

- Changed state executers abstract Run to a optional override
- Set unity minimal version to 2021.3

# [1.0.1] - 2023-05-04

### Added

- A default state
- Editor support for default state

### Fixed

- A bug where unity would return error when clicking state remove button when list is empty

# [1.0.0] - 2023-05-04

### Added

- Initial version 