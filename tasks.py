import hy
from invoke import task

@task
def dump(ctx):
    """
    Dump track metadata
    """
    from dump_track_meta import main
    main()

@task
def warnings(ctx):
    """
    Generate warnings for presence of traditional characters and lack of youtube links
    """
    from generate_warnings import main
    main()

@task
def words(ctx):
    """
    Generate word list from all lyrics
    """
    from generate_word_list import main
    main()
