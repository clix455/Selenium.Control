# Contributing to Clix

Thank you for considering contributing to the Clix project. There are a
number of ways you can help:

## Issue Reports

When opening new issues or commenting on existing issues please make
sure discussions are related to concrete technical issues with the
Clix software.

It's important that issue reports include the steps to reproduce
the defect, and clearly describe what results you are seeing and 
what results you expect.

## Feature Requests

If you are thinking of some cool new features, feel free to open an issue
with details describing what feature(s) you'd like added or changed.  

## Documentation

In progress.

## Contributions steps

The Clix project welcomes new contributors.

### Step 1: Fork

Fork the project [on Github](https://github.com/MinjetTech/Clix)
and check out your copy locally.

```shell
% git clone git@github.com:username/Clix.git
% cd Clix
% git remote add upstream git@github.com:MinjetTech/Clix.git
```

### Step 2: Branch

Create a feature branch and start hacking. A good branch name would be 
(where issue #3 is the ticket you're working on):

```shell
% git checkout -b 3-add-contribution-guide
```

We practice HEAD-based development, which means all changes are applied
directly on top of master.

### Step 3: Commit

First make sure git knows your name and email address:

```shell
% git config --global user.name 'Jane Jackson'
% git config --global user.email 'jane@example.com'
```

**Writing good commit messages is important.** A commit message
should describe what changed, why, and reference issues fixed (if
any). Follow these guidelines when writing one:

1. The first line should be around 50 characters or less and contain a
    short description of the change.
2. Keep the second line blank.
3. Wrap all other lines at 72 columns.
4. Include `Fixes #N`, where _N_ is the issue number the commit
    fixes, if any.

A good commit message can look like this:

```text
explain commit normatively in one line

Body of commit message is a few lines of text, explaining things
in more detail, possibly giving some background about the issue
being fixed, etc.

The body of the commit message can be several paragraphs, and
please do proper word-wrap and keep columns shorter than about
72 characters or so. That way `git log` will show things
nicely even when it is indented.

Fixes #141
```

The first line must be meaningful as it's what people see when they
run `git shortlog` or `git log --oneline`.

### Step 4: Rebase

Use `git rebase` (not `git merge`) to sync your work from time to time.

```shell
% git fetch upstream
% git rebase upstream/master
```

### Step 5: Test

Bug fixes and features **should have tests**. Look at other tests to
see how they should be structured.

Before you submit your pull request make sure you pass all the tests:

### Step 6: Push

```shell
% git push origin 3-add-contribution-guide
```

Go to https://github.com/yourname/Clix.git and press the _Pull
Request_ and fill out the form. 

Pull requests are usually reviewed within a few days. If there are
comments to address, apply your changes in new commits (preferably
[fixups](http://git-scm.com/docs/git-commit)) and push to the same
branch.

### Step 7: Integration

When code review is complete, a committer will take your PR and
integrate it on Clix's master branch. Because we like to keep a
linear history on the master branch, we will normally squash and rebase
your branch history.
