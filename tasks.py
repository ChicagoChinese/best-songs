import hy
from invoke import task

@task
def dump(ctx, playlist_name):
    """
    Dump metadata for all tracks in playlist
    """
    from dump_track_meta import main
    main(playlist_name)

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

@task
def page(ctx):
    """
    Generate HTML page listing for first 50 tracks
    """
    from generate_page import main
    main()

@task
def zip(ctx):
    """
    Generate ZIP file containing first 50 track files
    """
    from generate_zip import main
    main()
